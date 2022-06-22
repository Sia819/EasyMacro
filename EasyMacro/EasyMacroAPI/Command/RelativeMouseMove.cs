using System;
using EasyMacroAPI.Common;
using EasyMacroAPI.Model;

namespace EasyMacroAPI.Command
{
    public class RelativeMouseMove : IAction
    {
        public int X { get; set; }

        public int Y { get; set; }

        private FindWindowPosition findWindowPosition;

        public MacroTypes MacroType => MacroTypes.MouseClick;

        public RelativeMouseMove(string windowName, int x, int y)
        {
            this.X = x;
            this.Y = y;
            findWindowPosition = new FindWindowPosition(windowName);
        }

        public void ChangeWindowName(string windowName)
        {
            findWindowPosition.WindowName = windowName;
        }

        public void ChangeWindow(IntPtr windowName)
        {
            findWindowPosition.TargetWindow = windowName;
        }


        public void Do()
        {
            findWindowPosition.Do();
            int relativeX = findWindowPosition.ClientRect.Left + X;
            int relativeY = findWindowPosition.ClientRect.Top + Y;

            if (relativeX > findWindowPosition.ClientRect.Right)
                relativeX = findWindowPosition.ClientRect.Right;
            if (relativeY > findWindowPosition.ClientRect.Bottom)
                relativeY = findWindowPosition.ClientRect.Bottom;

            WinAPI.SetCursorPos(relativeX, relativeY);
        }
    }
}
