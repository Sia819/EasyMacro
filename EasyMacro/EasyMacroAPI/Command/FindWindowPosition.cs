using EasyMacroAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyMacroAPI.Common;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace EasyMacroAPI.Command
{
    public class FindWindowPosition : IAction
    {
        public MacroTypes MacroType => MacroTypes.FindWindowPosition;

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        public string WindowName { get; set; }

        public Rect rect = new Rect();

        public FindWindowPosition(string windowName)
        {
            WindowName = windowName;
        }

        public void Do()
        {
            Process[] processes = Process.GetProcessesByName(WindowName);
            Process lol = processes[0];
            IntPtr ptr = lol.MainWindowHandle;
            rect = new Rect();
            GetWindowRect(ptr, ref rect);
        }
    }
}
