using System.Runtime.InteropServices;
using System.Windows.Forms;
using EasyMacroAPI.Common;
using EasyMacroAPI.Model;

namespace EasyMacroAPI.Command
{
    public class InputMouse : IAction
    {

        public int X { get; private set; }
        public int Y { get; private set; }
        public MouseClickTypes MouseClickTypes { get; set; }

        public MacroTypes MacroType => MacroTypes.MouseClick;

        public InputMouse(MouseClickTypes presstype,int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.MouseClickTypes = presstype;

        }

        public void Do()
        {
            WinAPI.mouse_event((uint)MouseClickTypes, (uint)X, (uint)Y, 0, 0);
        }
    }
}
