using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using EasyMacroAPI.Model;

namespace EasyMacroAPI.Command
{
    public class FindWindowPosition : IAction
    {
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        public IntPtr TargetWindow { get; set; }

        #region Property - ClientRect
        public Rect ClientRect
        {
            get => clientRect;
            set => clientRect = value;
        }
        private Rect clientRect;
        #endregion

        public string WindowName { get; set; }

        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        public FindWindowPosition(string windowName)
        {
            if (windowName is not null && windowName != "")
            {
                ClientRect = new Rect();
                Process[] processes = Process.GetProcessesByName(WindowName);
                Process lol = processes[0];
                TargetWindow = lol.MainWindowHandle;
            }
        }

        public FindWindowPosition(IntPtr target)
        {
            TargetWindow = target;
        }

        public void Do()
        {
            GetWindowRect(TargetWindow, ref clientRect);
        }
    }
}
