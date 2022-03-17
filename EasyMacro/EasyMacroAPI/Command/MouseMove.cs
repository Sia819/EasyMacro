using System;
using EasyMacroAPI.Model;
using EasyMacroAPI.Common;
using System.Xml.Serialization;

namespace EasyMacroAPI.Command
{
    public class MouseMove : IAction
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public MacroTypes MacroType => MacroTypes.MouseMove;

        public MouseMove(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public void Do()
        {
            WinAPI.SetCursorPos(X, Y);
        }
    }


}
