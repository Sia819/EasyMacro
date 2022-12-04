using EasyMacroAPI.Model;
using EasyMacroAPI.Common;
using System.Drawing;

namespace EasyMacroAPI.Command
{
    public class MouseMove : IAction
    {
        public int X { get; set; }
        public int Y { get; set; }

        public MouseMove(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public MouseMove(Point point)
        {
            X = point.X;
            Y = point.Y;
        }

        public void Do()
        {
            WinAPI.SetCursorPos(X, Y);
        }
    }
}
