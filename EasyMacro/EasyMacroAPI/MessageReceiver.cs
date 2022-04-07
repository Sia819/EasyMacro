using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using EasyMacroAPI.Model;
using PInvoke;
using static PInvoke.User32;
using static PInvoke.User32.WindowStyles;
using static PInvoke.User32.WindowMessage;
using static EasyMacroAPI.Common.WinAPI;
using System.Threading;


namespace EasyMacroAPI
{
    /// <summary>
    /// C++ Invoked Windows Desktop Application
    /// </summary>
    internal class MessageReceiver
    {
        #region Public Properties
        private string name;
        public string Name
        {
            get
            {
                if (name == null)
                    return "Window";
                else
                    return name;
            }
            private set { name = value; }
        }

        public IntPtr Handle { get; private set; }
        #endregion

        #region Public Delegate
        public unsafe delegate void HotkeyDelegate();
        #endregion

        #region Private Structor
        private unsafe struct Hotkey
        {
            public Keys keys;
            public KeyModifiers keyModifiers;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public int mouseData; // be careful, this must be ints, not uints (was wrong before I changed it...). regards, cmew.
            public int flags;
            public int time;
            public UIntPtr dwExtraInfo;
        }
        #endregion

        #region Private Constant
        private const int MACRO_REG = 11111;
        private const int MACRO_UNREG = 22222;
        private const int NULL = 0;
        #endregion

        #region Private Member
        private MSG msg;
        private IntPtr g_hInst;
        private Dictionary<Hotkey, HotkeyData> registerdKeys = new Dictionary<Hotkey, HotkeyData>();
        private int hotkeyCount;
        private class HotkeyData
        {
            public int HotkeyID { get; set; }

            public HotkeyDelegate HotkeyDelegate { get; set; }
        }
        #endregion

        #region Constructor
        public unsafe MessageReceiver()
        {
            // Primary param init
            IntPtr hInstance = Marshal.GetHINSTANCE(typeof(MessageReceiver).Module);
            g_hInst = hInstance;

            // Initialization
            msg = new MSG();
            WNDCLASS WndClass = new WNDCLASS();
            WndClass.hInstance = hInstance;
            WndClass.lpfnWndProc = WndProc;
            WndClass.lpszClassName = ToCharPointer(Name);
            RegisterClass(ref WndClass);
        }

        public MessageReceiver(string windowName) : this()
        {
            Name = windowName;
        }

        ~MessageReceiver()
        {
            
        }
        #endregion

        #region Public Method
        public void Run()
        {
            new Thread(new ThreadStart(() =>
            {
                // If doesn't make CreateWindow, we can't get a Handle
                Handle = CreateWindow(Name,
                                      Name,
                                      WS_OVERLAPPEDWINDOW,
                                      CW_USEDEFAULT,
                                      CW_USEDEFAULT,
                                      CW_USEDEFAULT,
                                      CW_USEDEFAULT,
                                      IntPtr.Zero,
                                      IntPtr.Zero,
                                      g_hInst,
                                      IntPtr.Zero);

                MessageLoop();

            }))
            {
                // Thread Properties
                IsBackground = true
            }
            .Start();
        }

        public void AddHotkey(Keys key, KeyModifiers keyModifiers, HotkeyDelegate hotkeyDelegate)
        {
            PostMessage(this.Handle, (int)MACRO_REG, Marshal.GetFunctionPointerForDelegate(hotkeyDelegate), (IntPtr)MAKELPARAM((int)keyModifiers, (int)key));
        }

        public void RemoveHotkey(Keys key, KeyModifiers keyModifiers)
        {
            PostMessage(this.Handle, (int)MACRO_UNREG, 0, MAKELPARAM((int)keyModifiers, (int)key));
        }
        //TODO : 아래 함수는 메인함수에서 실행되어야 함.
        //SafeHookHandle mouseHook = SetWindowsHookEx(WH_MOUSE_LL, mouseHookProc, hInstance, 0);
        //TODO : 녹화 매크로 함수 구현

        #endregion

        #region Private Method
        private unsafe void MessageLoop()
        {
            fixed (MSG* messagePointer = &msg)
            {
                while (GetMessage(messagePointer, IntPtr.Zero, WM_NULL, WM_NULL) > 0)
                {
                    switch ((int)msg.message)
                    {
                        case (int)WM_HOTKEY:
                            {
                                Keys key = (Keys)(((int)msg.lParam >> 16) & 0xFFFF);
                                KeyModifiers modifier = (KeyModifiers)((int)msg.lParam & 0xFFFF);
                                Hotkey hotkey = new Hotkey { keys = key, keyModifiers = modifier };

                                if (registerdKeys.TryGetValue(hotkey, out HotkeyData value))
                                {
                                    value.HotkeyDelegate.Invoke();
                                }
                                break;
                            }

                        case (int)MACRO_REG:  // Custom RegisterHotKey message received
                            {
                                Keys key = (Keys)(((int)msg.lParam >> 16) & 0xFFFF);
                                KeyModifiers modifier = (KeyModifiers)((int)msg.lParam & 0xFFFF);
                                Hotkey hotkey = new Hotkey { keys = key, keyModifiers = modifier };

                                if (registerdKeys.TryGetValue(hotkey, out HotkeyData value))   // Get registered hotkey of id from Dictionary
                                {// Avoid duplicate registration
                                    break;
                                }
                                HotkeyDelegate h = (HotkeyDelegate)Marshal.GetDelegateForFunctionPointer(msg.wParam, typeof(HotkeyDelegate));
                                registerdKeys.Add(hotkey, new HotkeyData() { HotkeyID = hotkeyCount, HotkeyDelegate = h } );
                                RegisterHotKey(NULL, hotkeyCount, modifier, key);
                                hotkeyCount++;
                                break;
                            }
                        case MACRO_UNREG:  // Custom UnregisterHotKey message received
                            {
                                Keys key = (Keys)(((int)msg.lParam >> 16) & 0xFFFF);
                                KeyModifiers modifier = (KeyModifiers)((int)msg.lParam & 0xFFFF);
                                Hotkey hotkey = new Hotkey { keys = key, keyModifiers = modifier };
                                if (registerdKeys.TryGetValue(hotkey, out HotkeyData value))  // Get registered hotkey of id from Dictionary
                                {// Avoid duplicate unregistration
                                    UnregisterHotKey(NULL, value.HotkeyID);
                                    registerdKeys.Remove(hotkey); // Removed hotkeys can be register again
                                }
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
        }

        private unsafe IntPtr WndProc(IntPtr hWnd, WindowMessage msg, void* wParam, void* lParam)
        {
            switch (msg)
            {
                case WM_KEYDOWN:
                    return IntPtr.Zero;
                case WM_PAINT:
                    return IntPtr.Zero;
                case WM_DESTROY:
                    PostQuitMessage(0);     // GetMessage returns 0
                    return IntPtr.Zero;
                default:
                    return DefWindowProc(hWnd, msg, (IntPtr)wParam, (IntPtr)lParam);
            }
        }

        int mouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            MSLLHOOKSTRUCT p = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));

            POINT position;
            position.x = p.pt.x;
            position.y = p.pt.y;
            Console.WriteLine(position.x + ", " + position.y);

            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        private static unsafe char* ToCharPointer(string value)
        {
            fixed (char* strPtr = value) return strPtr;
        }

        private static int MAKELPARAM(int p, int p_2)
        {
            return ((p_2 << 16) | (p & 0xFFFF));
        }
        #endregion
    }
}



/*
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EasyMacroAPI.Common;
using static PInvoke.User32;
using static PInvoke.User32.WindowMessage;

using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace EasyMacroAPI
{
    internal unsafe class HotKey// : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RegisterHotKey(int hWnd, int id, int fsModifiers, Keys vk);

        [DllImport("user32.dll")]
        public static extern int UnregisterHotKey(int hwnd, int id);

        [DllImport("user32.dll")]
        public static extern int DispatchMessage(IntPtr lpMsg);
        [DllImport("user32.dll")]
        public static extern bool TranslateMessage(IntPtr lpMsg);
        [DllImport("user32.dll")]
        public static extern int GetMessage(IntPtr lpMsg, IntPtr hWnd, int wMsgFilterMin, int wMsgFilterMax);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int GetCurrentThreadId();

        const int HOTKEY_ID = 31197; //Any number to use to identify the hotkey instance

        const int WM_HOTKEY = 0x0312;
        public const int NULL = 0;
        public const int WM_NULL = 0;

        ///private ConcurrentQueue<Keys> q1;

        private Thread t1;
        private IntPtr threadHandle
        {
            get
            {
                return Process.GetCurrentProcess().Handle;
            }
        }
        public HotKey()
        {
            t1 = new Thread(new ThreadStart(eventLoop));
            t1.IsBackground = true;
            t1.Start();
        }

        private void eventLoop()
        {
            IntPtr hInstance = Marshal.GetHINSTANCE(typeof(HotKey).Module);
            
            RegisterHotKey(NULL, 1, NULL, Keys.A);   // Register HotKey A
            RegisterHotKey(NULL, 1, (int)Model.KeyModifiers.Control, Keys.B);   // Register HotKey A
            MSG _message = new MSG();                // point referenced of MSG structor
            IntPtr Message = (IntPtr)(&_message);

            while (GetMessage(Message, IntPtr.Zero, 0, 0) > 0)
            //while (PeekMessage(Message, IntPtr.Zero, 0, 0, 0))
            {
                TranslateMessage(Message);
                DispatchMessage(Message);

                Keys key = (Keys)(((int)_message.lParam >> 16) & 0xFFFF); 
                Model.KeyModifiers modifier = (Model.KeyModifiers)((int)_message.lParam & 0xFFFF);

                if ((int)_message.message == WM_HOTKEY)
                {
                    if (Model.KeyModifiers.None == modifier && Keys.F9 == key)
                    {
                        Console.WriteLine("F9");
                        MacroManager.Instance.StopMacro();
                    }
                    Console.WriteLine("WM_HOTKEY");
                }
                else if ((int)_message.message == (int)WM_MOUSEMOVE)
                {
                    Console.WriteLine("event Entered!!");
                }
                Console.WriteLine("발생");
            }
            UnregisterHotKey(NULL, 1);
        }

        public void RegisterHotKey()
        {
            ///q1.Enqueue(Keys.F9);
            // 핫키 등록
            //RegisterHotKey(NULL, HOTKEY_ID, NULL, Keys.F9);
            PostMessage((IntPtr)threadHandle, WindowMessage.WM_MOUSEMOVE, (IntPtr)0, (IntPtr)0);
        }

        public void UnregisterHotKey()
        {
            UnregisterHotKey(NULL, HOTKEY_ID);
        }
    }
}


*/


/*
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using EasyMacroAPI.Common;

namespace EasyMacroAPI
{
    internal class HotKey : Form
    {
        const int HOTKEY_ID = 31197; //Any number to use to identify the hotkey instance

        const int WM_HOTKEY = 0x0312;
        protected override void WndProc(ref Message message)
        {
            switch (message.Msg)
            {
                case WM_HOTKEY:
                    Keys key = (Keys)(((int)message.LParam >> 16) & 0xFFFF);
                    Model.KeyModifiers modifier = (Model.KeyModifiers)((int)message.LParam & 0xFFFF);
                    //MessageBox.Show("HotKey Pressed :" + modifier.ToString() + " " + key.ToString());

                    if (Model.KeyModifiers.None == modifier && Keys.F9 == key)
                    {
                        MacroManager.Instance.StopMacro();
                        MessageBox.Show(new Form { TopMost = true }, "HotKey Pressed!");
                    }
                    break;
            }
            base.WndProc(ref message);
        }

        public void RegisterHotKey()
        {
            // 핫키 등록
            //RegisterHotKey((IntPtr)handle, HOTKEY_ID, KeyModifiers.Control | KeyModifiers.Shift, Keys.N);
            WinAPI.RegisterHotKey(this.Handle, HOTKEY_ID, Model.KeyModifiers.None, Keys.F9);
        }

        public void UnregisterHotKey()
        {
            //핫키 해제
            WinAPI.UnregisterHotKey(this.Handle, HOTKEY_ID);
        }
    }
}
*/