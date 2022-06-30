using System;
using EasyMacroAPI.Model;
using System.Runtime.InteropServices;

namespace EasyMacroAPI.Common
{
    public static class WinAPI
    {
        #region DLL Import

        /// <summary> Low-Level 마우스 입력 제어 </summary>
        [DllImport("user32.dll")] 
        internal static extern void mouse_event(uint dwFlags, uint dx, uint dy, int dwData, int dwExtraInfo);

        
        /// <summary> 마우스 커서의 위치를 지정합니다. </summary>
        [DllImport("user32.dll")]
        internal static extern int SetCursorPos(int x, int y);

        
        /// <summary> 키보드 이벤트를 발생시킵니다. </summary>
        [DllImport("user32.dll")]
        internal static extern void keybd_event(byte vk, byte scan, int flags, int extrainfo);

        
        /// <summary> 핫키를 등록합니다 </summary>
        [DllImport("user32.dll")]
        internal static extern bool RegisterHotKey(int hWnd, int id, KeyModifiers fsModifiers, Keys vk);


        /// <summary> 등록한 핫키를 해제합니다 </summary>
        [DllImport("user32.dll")]
        internal static extern bool UnregisterHotKey(int hWnd, int id);


        /// <summary> 윈도우 메시지를 발생시킵니다 </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool PostMessage(IntPtr hWnd, int id, int fsModifiers, int vk);


        /// <summary> 윈도우 메시지를 발생시킵니다 </summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool PostMessage(IntPtr hWnd, int id, IntPtr fsModifiers, IntPtr vk);


        /// <summary>  </summary>
        [System.Runtime.InteropServices.DllImport("User32", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);


        /// <summary>  </summary>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        /// <summary>  </summary>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref RECT rectangle);

        public struct RECT
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        /// <summary>  </summary>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator System.Drawing.PointPoint(POINT point) => new System.Drawing.Point(point.X, point.Y);
        }

        #endregion
    }
}
