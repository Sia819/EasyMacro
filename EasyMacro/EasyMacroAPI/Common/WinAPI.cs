using System;
using EasyMacroAPI.Model;

namespace EasyMacroAPI.Common
{
    public class WinAPI
    {
        // public const uint MOUSEEVENTF_MOVE = 0x0001;
        // public const uint LBDOWN = 0x00000002;      // 왼쪽 마우스 버튼 눌림
        // public const uint LBUP = 0x00000004;        // 왼쪽 마우스 버튼 떼어짐
        // public const uint RBDOWN = 0x00000008;      // 오른쪽 마우스 버튼 눌림
        // public const uint RBUP = 0x000000010;       // 오른쪽 마우스 버튼 떼어짐
        // public const uint MBDOWN = 0x00000020;      // 휠 버튼 눌림
        // public const uint MBUP = 0x000000040;       // 휠 버튼 떼어짐
        // public const uint WHEEL = 0x00000800;       // 휠 스크롤

        #region DLL Import

        [System.Runtime.InteropServices.DllImport("user32.dll")] // 입력 제어
        internal static extern void mouse_event(uint dwFlags, uint dx, uint dy, int dwData, int dwExtraInfo);

        /// <summary>
        /// 마우스 커서의 위치를 지정합니다.
        /// </summary>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern int SetCursorPos(int x, int y);

        /// <summary>
        /// 키보드 이벤트를 발생시킵니다.
        /// </summary>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern void keybd_event(byte vk, byte scan, int flags, int extrainfo);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool RegisterHotKey(int hWnd, int id, KeyModifiers fsModifiers, Keys vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool UnregisterHotKey(int hWnd, int id);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        internal static extern bool PostMessage(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        internal static extern bool PostMessage(IntPtr hWnd, int id, IntPtr fsModifiers, IntPtr vk);

        #endregion
    }
}
