// 훅을 담당하는 라이브러리 입니다.
namespace HookLib
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using EasyMacroAPI.Model;
    using static PInvoke.User32;
    using static PInvoke.Extension.User32;
    using PInvoke;
    using System.Windows.Threading;

    public class GlobalKeyboardHook : IMessageReceiver
    {
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

        //public static int hookHandle = 0;
        private static SafeHookHandle hookHandle = null;
        private static WindowsHookDelegate callbackDelegate;

        // 모든 감시할 키보드 키
        private static Dictionary<Keys, bool> keyboardKeyStatus = null;
        // 등록된 핫키에 대응하는 델리게이트
        private static Dictionary<(Keys, KeyModifiers), IMessageReceiver.HotkeyDelegate> registeredHotkey = null;

        public static void StartKeyboardHook()
        {
            callbackDelegate = new WindowsHookDelegate(KeyboardCallBack);
            if (hookHandle is not null)
            {
                if (!hookHandle.IsInvalid || !hookHandle.IsClosed)
                    return;
            }
            if (keyboardKeyStatus == null)
            {
                keyboardKeyStatus = new Dictionary<Keys, bool>();
                foreach (Keys key in Enum.GetValues(typeof(Keys)))
                {
                    if (keyboardKeyStatus.ContainsKey(key) is not true) keyboardKeyStatus.Add(key, false);
                }
            }
            if (registeredHotkey == null)
            {
                registeredHotkey = new Dictionary<(Keys, KeyModifiers), IMessageReceiver.HotkeyDelegate>();
            }

            hookHandle = SetWindowsHookEx(User32.WindowsHookType.WH_KEYBOARD_LL, callbackDelegate, IntPtr.Zero, 0);  // 키보드 훅
        }

        public void StopKeyboardHook()
        {
            hookHandle.Dispose();
        }

        public static int KeyboardCallBack(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (lParam != IntPtr.Zero)
            {
                KeyboardHookStruct? keyboardInput = Marshal.PtrToStructure<KeyboardHookStruct>(lParam);

                if (keyboardInput != null)
                {
                    Keys key = (Keys)keyboardInput.vkCode;
                    bool push = ((int)wParam == (int)User32.WindowMessage.WM_KEYDOWN || (int)wParam == (int)User32.WindowMessage.WM_SYSKEYDOWN) ? true : false;

                    keyboardKeyStatus[key] = push;  // 상태변경

                    // Debug
                    Console.WriteLine("(Key : {0}, push : {1}", key, push);

                    foreach (KeyValuePair<(Keys, KeyModifiers), IMessageReceiver.HotkeyDelegate> pair in registeredHotkey)
                    {
                        // TODO : KeyModifiers가 동시에 두개 들어오면 구분문제?
                        if (keyboardKeyStatus[pair.Key.Item1] && keyboardKeyStatus[KeyModifiersToKeyConverter(pair.Key.Item2)])
                        {
                            pair.Value.Invoke();
                        }
                    }

                }
            }
            return CallNextHookEx(hookHandle.DangerousGetHandle(), nCode, wParam, lParam);
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

    public static class GlobalMouseKeyHook
    {
        // GlobalMouseKeyHook는 EasyMacroAPI 에서 요구하지 않기 때문에 EasyMacroAPI.IMessageReceiver 인터페이스를 구현하지 않아도 됩니다.

        /// <summary> SafeHandle of Hook from Windows API, received by "SetWindowsHookEx" method. </summary>
        public static SafeHookHandle hookHandle = null;

        // RegisterMouseHotkey 함수로, 이 클래스에 등록되는 콜백함수들은 좌표를 반환합니다.
        public delegate void HotkeyDelegate(POINT point);

        private static WindowsHookDelegate callbackDelegate = null;
        private static Dictionary<mouse_status, bool> mouseKeyStatus = null;
        private static Dictionary<mouse_status, HotkeyDelegate> registeredHotkey = null;
        private static Dispatcher dispatcher = null;

        /// <summary> 마우스 훅을 시작합니다. </summary>
        public static void StartMouseHook()
        {
            // Dupe started check
            if (hookHandle is not null) return;

            // Initialize static member
            if (callbackDelegate is null) callbackDelegate = new(MouseCallBack);
            if (mouseKeyStatus is null)
            {
                mouseKeyStatus = new();
                foreach (mouse_status pair in Enum.GetValues(typeof(mouse_status)))
                    if (pair != mouse_status.NotSupport) mouseKeyStatus.Add(pair, false);

            }
            if (registeredHotkey is null) registeredHotkey = new();

            hookHandle = SetWindowsHookEx(User32.WindowsHookType.WH_MOUSE_LL, callbackDelegate, IntPtr.Zero, 0);  // 키보드 훅
        }

        /// <summary> 마우스 훅을 종료합니다. </summary>
        public static void StopMouseHook()
        {
            hookHandle.Dispose();   // like cpp delete handle, and this calls "UnhookWindowsHookEx" method.
            hookHandle = null;      // recycle value when "StartMouseHook" recalled
        }

        private static int MouseCallBack(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (lParam != IntPtr.Zero)
            {
                MSLLHOOKSTRUCT mouseInput = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);     // mouse data of pt, wheel, timespan and ect
                mouse_status status = windowMessageToMouseStatus((WindowMessage)(int)wParam, out bool isPush);   // mouse key

                mouseKeyStatus[status] = isPush;    // 상태변경

                foreach (KeyValuePair<mouse_status, HotkeyDelegate> i in registeredHotkey)  // check registerd hotkey delegate
                {
                    if (mouseKeyStatus[i.Key] == true) // registered key has pushed
                    {
                        if (dispatcher is null) i.Value.Invoke(mouseInput.pt);
                        else dispatcher.BeginInvoke(i.Value, mouseInput.pt);
                        mouseKeyStatus[i.Key] = false;
                    }
                }

                //Debug
                // Console.WriteLine(
                //     "(pt:({0}, {1}), mouseData:{2}, flags:{3}, time:{4}, dwExtraInfo:{5}",
                //     mouseInput.pt.x,
                //     mouseInput.pt.y,
                //     mouseInput.mouseData,
                //     mouseInput.flags,
                //     mouseInput.time,
                //     mouseInput.dwExtraInfo);
            }
            return CallNextHookEx(hookHandle.DangerousGetHandle(), nCode, wParam, lParam);
        }

        /// <summary>
        /// Hook 작업이 종료되어도, 등록된 Delegate가 제거되지 않으므로 사용시 주의가 필요합니다.
        /// </summary>
        public static void RegisterMouseHotkey(mouse_status status, HotkeyDelegate hotkeyDelegate)
        {
            if (registeredHotkey.ContainsKey(status) is not true)   // Dupe regist check
                registeredHotkey.Add(status, hotkeyDelegate);
        }
        
        public static void UnregisterMouseHotkey(mouse_status status)
        {
            registeredHotkey.Remove(status);
        }



        public static void SetDispatcher(Dispatcher dispatcher)
        {
            GlobalMouseKeyHook.dispatcher = dispatcher;
        }


        // https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-msllhookstruct
        [StructLayout(LayoutKind.Sequential)]
        public struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public int mouseData;
            public int flags;
            public int time;
            public UIntPtr dwExtraInfo;
        }

        private static mouse_status windowMessageToMouseStatus(WindowMessage message, out bool isPush)
        {
            switch (message)
            {
                case WindowMessage.WM_LBUTTONDOWN: isPush = true; return mouse_status.Left;
                case WindowMessage.WM_LBUTTONUP: isPush = false; return mouse_status.Left;
                case WindowMessage.WM_MBUTTONDOWN: isPush = true; return mouse_status.Middle;
                case WindowMessage.WM_MBUTTONUP: isPush = false; return mouse_status.Middle;
                case WindowMessage.WM_RBUTTONDOWN: isPush = true; return mouse_status.Right;
                case WindowMessage.WM_RBUTTONUP: isPush = false; return mouse_status.Right;
                case WindowMessage.WM_MOUSEWHEEL: isPush = false; return mouse_status.Wheel;
                default: isPush = false; return mouse_status.NotSupport;
            }
        }

        public enum mouse_status
        {
            NotSupport = -1,
            Left,
            Middle,
            Right,
            Wheel,
            X1,
            X2
        }
    }
}