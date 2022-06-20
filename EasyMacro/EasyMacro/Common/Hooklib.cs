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

    public static class GlobalKeyboardHook
    {
        private static SafeHookHandle hookHandle = null;

        private static WindowsHookDelegate callbackDelegate = null;
        // 모든 감시할 키보드 키
        private static Dictionary<Keys, bool> keyboardKeyStatus = null;
        // 등록된 핫키에 대응하는 델리게이트
        private static Dictionary<(Keys, KeyModifiers), KeyboardHotkeyDelegate> registeredHotkey = null;

        public delegate void KeyboardHotkeyDelegate(Keys keys, KeyModifiers keyModifiers = KeyModifiers.None);

        public static void AddKeyboardHotkey(Keys keys, KeyModifiers keyModifiers, KeyboardHotkeyDelegate hotkeyDelegate)
        {
            if (registeredHotkey.ContainsKey((keys, keyModifiers)) is not true)   // Dupe regist check
                registeredHotkey.Add((keys, keyModifiers), hotkeyDelegate);
        }

        public static void RemoveKeyboardHotkey(Keys keys, KeyModifiers keyModifiers)
        {
            registeredHotkey.Remove((keys, keyModifiers));
        }

        private static Keys KeyModifiersToKeyConverter(KeyModifiers keyModifiers)
        {
            switch (keyModifiers)
            {
                case KeyModifiers.None: return Keys.None;
                case KeyModifiers.Control: return Keys.Control;    //return Keys.LeftCtrl;
                case KeyModifiers.Alt: return Keys.Alt;    //return Keys.LeftAlt;
                case KeyModifiers.Shift: return Keys.Shift;  //return Keys.LeftShift;
                case KeyModifiers.Windows: return Keys.LWin;

                default: return Keys.None;
            }
        }

        private static KeyModifiers KeyToKeyModifiersConverter(Keys keys)
        {
            switch (keys)
            {
                case Keys.None: return KeyModifiers.None;
                case Keys.Control: return KeyModifiers.Control;    //return Keys.LeftCtrl;
                case Keys.ControlKey: return KeyModifiers.Control;    //return Keys.LeftCtrl;
                case Keys.LControlKey: return KeyModifiers.Control;    //return Keys.LeftCtrl;
                case Keys.RControlKey: return KeyModifiers.Control;    //return Keys.LeftCtrl;
                case Keys.Shift: return KeyModifiers.Shift;  //return Keys.LeftShift;
                case Keys.ShiftKey: return KeyModifiers.Shift;  //return Keys.LeftShift;
                case Keys.LShiftKey: return KeyModifiers.Shift;  //return Keys.LeftShift;
                case Keys.RShiftKey: return KeyModifiers.Shift;  //return Keys.LeftShift;
                case Keys.Alt: return KeyModifiers.Alt;    //return Keys.LeftAlt;
                case Keys.LWin: return KeyModifiers.Windows;
                case Keys.RWin: return KeyModifiers.Windows;
                case Keys.CapsLock: return KeyModifiers.CapsLock;

                default: return KeyModifiers.NotSupport;
            }
        }

        private static bool KeyModifiersPressedFlagsCheck(KeyModifiers keyModifiers)
        {
            KeyModifiers pressedState = KeyModifiers.None;
            if (keyboardKeyStatus[Keys.Control]
                || keyboardKeyStatus[Keys.ControlKey]
                || keyboardKeyStatus[Keys.LControlKey]
                || keyboardKeyStatus[Keys.RControlKey])
            { 
                pressedState |= KeyModifiers.Control;
            }
            if (keyboardKeyStatus[Keys.Shift]
                || keyboardKeyStatus[Keys.ShiftKey]
                || keyboardKeyStatus[Keys.LShiftKey]
                || keyboardKeyStatus[Keys.RShiftKey])
            {
                pressedState |= KeyModifiers.Shift;
            }
            if (keyboardKeyStatus[Keys.Alt]
                || keyboardKeyStatus[Keys.LMenu]
                || keyboardKeyStatus[Keys.RMenu]
                || keyboardKeyStatus[Keys.Menu])
            {
                pressedState |= KeyModifiers.Alt;
            }
            if (keyboardKeyStatus[Keys.LWin]
                || keyboardKeyStatus[Keys.RWin])
            {
                pressedState |= KeyModifiers.Windows;
            }
            if (keyboardKeyStatus[Keys.CapsLock])
            {
                pressedState |= KeyModifiers.CapsLock;
            }
            return (pressedState == keyModifiers);
        }

        private static bool IsKeyModifiersPressed()
        {
            return keyboardKeyStatus[Keys.Control]
                    || keyboardKeyStatus[Keys.ControlKey]
                    || keyboardKeyStatus[Keys.LControlKey]
                    || keyboardKeyStatus[Keys.RControlKey]
                    || keyboardKeyStatus[Keys.Shift]
                    || keyboardKeyStatus[Keys.ShiftKey]
                    || keyboardKeyStatus[Keys.LShiftKey]
                    || keyboardKeyStatus[Keys.RShiftKey]
                    || keyboardKeyStatus[Keys.Alt]
                    || keyboardKeyStatus[Keys.LMenu]
                    || keyboardKeyStatus[Keys.RMenu]
                    || keyboardKeyStatus[Keys.Menu]
                    || keyboardKeyStatus[Keys.LWin]
                    || keyboardKeyStatus[Keys.RWin]
                    || keyboardKeyStatus[Keys.CapsLock];
        }



        public static void StartKeyboardHook()
        {
            if (hookHandle is not null) return;
            if (callbackDelegate is null) callbackDelegate = new WindowsHookDelegate(KeyboardCallBack);
            if (registeredHotkey == null) registeredHotkey = new();
            // 키보드 버튼 상태 딕셔너리 초기화
            if (keyboardKeyStatus is null)
            {
                keyboardKeyStatus = new();
                foreach (Keys key in Enum.GetValues(typeof(Keys)))
                {
                    if (keyboardKeyStatus.ContainsKey(key) is not true) keyboardKeyStatus.Add(key, false);
                }
            }

            hookHandle = SetWindowsHookEx(User32.WindowsHookType.WH_KEYBOARD_LL, callbackDelegate, IntPtr.Zero, 0);  // 키보드 훅
        }

        public static void StopKeyboardHook()
        {
            hookHandle.Dispose();   // Hook 핸들 해제
            hookHandle = null;      // Hook 중이 아닙니다.
        }

        public static int KeyboardCallBack(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (lParam != IntPtr.Zero)
            {
                KeyboardHookStruct keyboardInput = Marshal.PtrToStructure<KeyboardHookStruct>(lParam);
                Keys key = (Keys)keyboardInput.vkCode;

                // 키보드 키 상태 상시 체크 Dictionary 상태 변경
                bool push = ((int)wParam == (int)User32.WindowMessage.WM_KEYDOWN || (int)wParam == (int)User32.WindowMessage.WM_SYSKEYDOWN) ? true : false;
                keyboardKeyStatus[key] = push;

                // 실행할 핫키 Dictionary에 등록된 델리게이트를 실행
                foreach (KeyValuePair<(Keys, KeyModifiers), KeyboardHotkeyDelegate> pair in registeredHotkey)
                {
                    bool InvokeRequire = false;
                    if (keyboardKeyStatus[pair.Key.Item1]) // 해당되는 키가 눌려짐.
                    {
                        if (pair.Key.Item2 == KeyModifiers.None && !IsKeyModifiersPressed()) InvokeRequire = true; // None일 경우, 어떤 조합키도 눌려지면 안됨.
                        else if (pair.Key.Item2 != KeyModifiers.None && KeyModifiersPressedFlagsCheck(pair.Key.Item2)) InvokeRequire = true; // None이 아닌경우, 해당하는 키가 눌러져야됨.
                    }
                    else if (pair.Key.Item1 == Keys.AnyKey) InvokeRequire = true; // 아무 키나 눌렸을 때 동작하도록 설정된 경우

                    if (InvokeRequire)
                    {
                        pair.Value.Invoke(key, pair.Key.Item2);
                    }
                }

                // Debug - Console
                Console.WriteLine("(Key : {0}, push : {1}", key, push);
            }
            return CallNextHookEx(hookHandle.DangerousGetHandle(), nCode, wParam, lParam);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KeyboardHookStruct
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
        /// <summary> SafeHandle of Hook from Windows API, received by "SetWindowsHookEx" method. </summary>
        public static SafeHookHandle hookHandle = null;

        // RegisterMouseHotkey 함수로, 이 클래스에 등록되는 콜백함수들은 좌표를 반환합니다.
        public delegate void MouseHotkeyDelegate(POINT point);

        private static WindowsHookDelegate callbackDelegate = null;
        private static Dictionary<mouse_button, bool> mouseKeyStatus = null;
        private static Dictionary<mouse_button, MouseHotkeyDelegate> registeredHotkey = null;
        private static Dispatcher dispatcher = null;

        /// <summary> 마우스 훅을 시작합니다. </summary>
        public static void StartMouseHook()
        {
            // Dupe started check
            if (hookHandle is not null) return;
            // Initialize static member
            if (callbackDelegate is null) callbackDelegate = new WindowsHookDelegate(MouseCallBack);
            if (registeredHotkey is null) registeredHotkey = new();
            // 마우스 버튼 상태 딕셔너리 초기화
            if (mouseKeyStatus is null)
            {
                mouseKeyStatus = new();
                foreach (mouse_button pair in Enum.GetValues(typeof(mouse_button)))
                {
                    if (pair != mouse_button.NotSupport) mouseKeyStatus.Add(pair, false);
                }
            }


            hookHandle = SetWindowsHookEx(User32.WindowsHookType.WH_MOUSE_LL, callbackDelegate, IntPtr.Zero, 0);  // 마우스 훅
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

                // 마우스 키 상태 상시 체크 Dictionary 상태 변경
                mouse_button status = windowMessageToMouseStatus((WindowMessage)(int)wParam, out bool isPush);
                mouseKeyStatus[status] = isPush;

                // 실행할 핫키 Dictionary에 등록된 델리게이트를 실행
                foreach (KeyValuePair<mouse_button, MouseHotkeyDelegate> i in registeredHotkey)  // check registerd hotkey delegate
                {
                    if (mouseKeyStatus[i.Key] == true) // registered key has pushed
                    {
                        if (dispatcher is null) i.Value.Invoke(mouseInput.pt);
                        else dispatcher.BeginInvoke(i.Value, mouseInput.pt);
                        mouseKeyStatus[i.Key] = false;
                    }
                    else if (i.Key == mouse_button.AnyKey)
                    {// 아무 키나 눌렸을 때 동작하도록 설정된 경우
                        i.Value.Invoke(mouseInput.pt);
                    }
                }

                //Debug - Console
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
        public static void AddMouseHotkey(mouse_button button, MouseHotkeyDelegate hotkeyDelegate)
        {
            if (registeredHotkey.ContainsKey(button) is not true)   // Dupe regist check
                registeredHotkey.Add(button, hotkeyDelegate);
        }

        public static void RemoveMouseHotkey(mouse_button status)
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

        private static mouse_button windowMessageToMouseStatus(WindowMessage message, out bool isPush)
        {
            switch (message)
            {
                case WindowMessage.WM_LBUTTONDOWN: isPush = true; return mouse_button.Left;
                case WindowMessage.WM_LBUTTONUP: isPush = false; return mouse_button.Left;
                case WindowMessage.WM_MBUTTONDOWN: isPush = true; return mouse_button.Middle;
                case WindowMessage.WM_MBUTTONUP: isPush = false; return mouse_button.Middle;
                case WindowMessage.WM_RBUTTONDOWN: isPush = true; return mouse_button.Right;
                case WindowMessage.WM_RBUTTONUP: isPush = false; return mouse_button.Right;
                case WindowMessage.WM_MOUSEWHEEL: isPush = false; return mouse_button.Wheel;
                default: isPush = false; return mouse_button.NotSupport;
            }
        }

        public enum mouse_button
        {
            AnyKey = -2,
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