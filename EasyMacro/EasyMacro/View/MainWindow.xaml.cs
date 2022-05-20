using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
using PInvoke;
using static PInvoke.User32;
using EasyMacroAPI.Model;
using System.Runtime.InteropServices;
using System.Reflection;

namespace EasyMacro.View
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            HookLib.GlobalKeyboardHook globalKeyboardHook = new HookLib.GlobalKeyboardHook();
            EasyMacroAPI.MacroManager.Instance.RegisterMessageReceiver(globalKeyboardHook);
            HookLib.GlobalKeyboardHook.StartHook();
        }

        void Hello()
        {
            Console.WriteLine("Hello");
        }

        private void mainFrame_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateFrameDataContext(sender, null);
        }

        private void mainFrame_LoadCompleted(object sender, NavigationEventArgs e)
        {
            UpdateFrameDataContext(sender, e);
        }

        private void UpdateFrameDataContext(object sender, NavigationEventArgs e)
        {

            var content = (sender as Frame).Content as FrameworkElement;
            if (content == null)
                return;
            content.DataContext = (sender as Frame).DataContext;

        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;

            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    this.mainFrame.Source = new Uri("pack://application:,,,/View/ListEditPage.xaml", UriKind.Absolute);
                }
                else
                {
                    this.mainFrame.Source = new Uri("pack://application:,,,/View/NodeEditPage.xaml", UriKind.Absolute);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }
    }

    public partial class MainWindow : MetroWindow
    {
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
        }
    }
}

namespace HookLib
{
    using System;
    using System.Runtime.InteropServices;

    public class GlobalKeyboardHook : IMessageReceiver
    {
        #region DllImport
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);
        #endregion

        #region IMessageReceiver 인터페이스 구현체
        public bool IsConfigured => true;
        public void AddHotkey(Keys keys, KeyModifiers keyModifiers, IMessageReceiver.HotkeyDelegate hotkeyDelegate)
        {
            registeredHotkey.Add((keys, keyModifiers), hotkeyDelegate);
        }
        public void RemoveHotkey(Keys keys, KeyModifiers keyModifiers)
        {
            registeredHotkey.Remove((keys, keyModifiers));
        }
        #endregion

        private static Keys KeyModifiersToKeyConverter(KeyModifiers keyModifiers)
        {
            switch (keyModifiers)
            {
                case KeyModifiers.None:
                    return Keys.None;
                case KeyModifiers.Alt:
                    //return Keys.LeftAlt;
                    return Keys.Alt;
                case KeyModifiers.Control:
                    //return Keys.LeftCtrl;
                    return Keys.Alt;
                case KeyModifiers.Shift:
                    //return Keys.LeftShift;
                    return Keys.Shift;
                case KeyModifiers.Windows:
                    return Keys.LWin;

                default:
                    return Keys.None;
            }
        }

        public static int hookHandle = 0;

        private static HookProc callbackDelegate;

        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        public const int WH_KEYBOARD_LL = 13;

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;

        private static Dictionary<Keys, bool> keyStatus = null;
        private static Dictionary<(Keys, KeyModifiers), IMessageReceiver.HotkeyDelegate> registeredHotkey = null;

        public static void StartHook()
        {
            callbackDelegate = new HookProc(CallBack);
            if (hookHandle != 0)
            {
                return;
            }
            if (keyStatus == null)
            {
                keyStatus = new Dictionary<Keys, bool>();
                foreach (Keys key in Enum.GetValues(typeof(Keys)))
                {
                    if (keyStatus.ContainsKey(key) is not true) keyStatus.Add(key, false);
                }
            }
            if (registeredHotkey == null)
            {
                registeredHotkey = new Dictionary<(Keys, KeyModifiers), IMessageReceiver.HotkeyDelegate>();
            }

            hookHandle = GlobalKeyboardHook.SetWindowsHookEx(WH_KEYBOARD_LL, callbackDelegate, IntPtr.Zero, 0);  // 키보드 훅
        }

        public static void StopHook()
        {
            GlobalKeyboardHook.UnhookWindowsHookEx(hookHandle);
        }

        public static int CallBack(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (lParam != IntPtr.Zero)
            {
                KeyboardHookStruct? keyboardInput = Marshal.PtrToStructure<KeyboardHookStruct>(lParam);

                if (keyboardInput != null)
                {
                    
                    Keys key = (Keys)keyboardInput.vkCode;

                    bool push = ((int)wParam == WM_KEYDOWN || (int)wParam == WM_SYSKEYDOWN) ? true : false;

                    if (keyStatus[key] != push) keyStatus[key] = push;  // 사전 설정

                    // Debug
                    Console.WriteLine("(Key : " + key + ", push : " + push);

                    foreach (KeyValuePair<(Keys, KeyModifiers), IMessageReceiver.HotkeyDelegate> pair in registeredHotkey)
                    {
                        // KeyModifiers가 동시에 두개 들어오면 구분문제?
                        if (keyStatus[pair.Key.Item1] && keyStatus[KeyModifiersToKeyConverter(pair.Key.Item2)])
                        {
                            pair.Value.Invoke();
                        }
                    }

                }
            }
            return GlobalKeyboardHook.CallNextHookEx(hookHandle, nCode, wParam, lParam);
        }



        [StructLayout(LayoutKind.Sequential)]
        private class KeyboardHookStruct
        {
            /// <summary>
            /// Specifies a virtual-key code. The code must be a value in the range 1 to 254.
            /// 가상 키 코드를 지정합니다. 코드는 1에서 254 사이의 값이어야 합니다.
            /// </summary>
            public int vkCode;
            /// <summary>
            /// Specifies a hardware scan code for the key.
            /// 키에 대한 하드웨어 스캔 코드를 지정합니다.
            /// </summary>
            public int scanCode;
            /// <summary>
            /// Specifies the extended-key flag, event-injected flag, context code, and transition-state flag.
            /// 확장 키 플래그, 이벤트 주입 플래그, 컨텍스트 코드 및 전환 상태 플래그를 지정합니다.
            /// </summary>
            public int flags;
            /// <summary>
            /// Specifies the time stamp for this message.
            /// 이 메시지의 타임스탬프를 지정합니다.
            /// </summary>
            public int time;
            /// <summary>
            /// Specifies extra information associated with the message.
            /// 메시지와 관련된 추가 정보를 지정합니다.
            /// </summary>
            public int dwExtraInfo;
        }

    }
}
