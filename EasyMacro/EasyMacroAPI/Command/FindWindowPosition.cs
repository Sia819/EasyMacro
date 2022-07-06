using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using EasyMacroAPI.Model;
using static EasyMacroAPI.Common.WinAPI;

namespace EasyMacroAPI.Command
{
    public class FindWindowPosition : IAction
    {
        public IntPtr TargetWindow { get; set; }

        #region Property - ClientRect
        public RECT ClientRect
        {
            get => clientRect;
            set => clientRect = value;
        }
        private RECT clientRect;
        #endregion

        public string WindowName { get; set; }

        public FindWindowPosition(string windowName)
        {
            if (windowName is not null && windowName != "")
            {
                ClientRect = new RECT();
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
