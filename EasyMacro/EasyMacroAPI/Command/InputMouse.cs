using System.Runtime.InteropServices;
using System.Windows.Forms;
using EasyMacroAPI.Common;
using EasyMacroAPI.Model;

namespace EasyMacroAPI.Command
{
    public class InputMouse : IAction
    {
        public int X { get; set; }

        public int Y { get; set; }

        public MouseClickTypes MouseClickType { get; set; }

        public MacroTypes MacroType => MacroTypes.MouseClick;

        public InputMouse(MouseClickTypes presstype, int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.MouseClickType = presstype;
        }

        public void Do()
        {
            //WinAPI.mouse_event((uint)MouseClickTypes.MOUSEEVENTF_MOVE, (uint)X, (uint)Y, 0, 0);
            WinAPI.mouse_event((uint)MouseClickType, (uint)X, (uint)Y, 0, 0);
        }
    }
}
