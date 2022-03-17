using EasyMacroAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMacroAPI.Command
{
    public class MouseClick : IAction
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public MacroTypes MacroType => MacroTypes.MouseClick;

        public MouseClick(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public void Do()
        {
            
        }


    }
}
