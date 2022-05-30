﻿using System.Runtime.InteropServices;
using System.Windows.Forms;
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


        public void Do()
        {
            findWindowPosition.Do();
            int relativeX = findWindowPosition.rect.Left + X;
            int relativeY = findWindowPosition.rect.Top + Y;

            if (relativeX > findWindowPosition.rect.Right)
                relativeX = findWindowPosition.rect.Right;
            if (relativeY > findWindowPosition.rect.Bottom)
                relativeY = findWindowPosition.rect.Bottom;

            WinAPI.SetCursorPos(relativeX, relativeY);
        }
    }
}