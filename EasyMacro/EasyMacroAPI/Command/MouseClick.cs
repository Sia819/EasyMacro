using EasyMacroAPI.Common;
using EasyMacroAPI.Model;
using System.Threading;

namespace EasyMacroAPI.Command
{
    public class MouseClick : IAction
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Delay { get; set; }

        public MacroTypes MacroType => MacroTypes.MouseClick;

        public MouseClick(int x, int y, int delay = 40)
        {
            this.X = x;
            this.Y = y;
            this.Delay = delay;
        }

        public void Do()
        {
            WinAPI.SetCursorPos(X, Y);
            WinAPI.mouse_event((uint)MouseClickTypes.LBDOWN, (uint)0, (uint)0, 0, 0);
            Thread.Sleep(Delay);
            WinAPI.mouse_event((uint)MouseClickTypes.LBUP, (uint)0, (uint)0, 0, 0);
        }
    }
}
