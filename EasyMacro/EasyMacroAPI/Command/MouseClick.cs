using EasyMacroAPI.Common;
using EasyMacroAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasyMacroAPI.Command
{
    public class MouseClick : IAction
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Delay { get; private set; }

        public MacroTypes MacroType => MacroTypes.MouseClick;

        public MouseClick(int x, int y, int delay = 40)
        {
            this.X = x;
            this.Y = y;
            this.Delay = delay;
        }

        public void Do()
        {
            WinAPI.mouse_event(WinAPI.LBDOWN,(uint)X,(uint)Y, 0, 0);
            Thread.Sleep(Delay);
            WinAPI.mouse_event(WinAPI.LBUP, (uint)X, (uint)Y, 0, 0);
        }
    }
}
