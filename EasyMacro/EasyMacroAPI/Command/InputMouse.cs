using EasyMacroAPI.Common;
using EasyMacroAPI.Model;

namespace EasyMacroAPI.Command
{
    public class InputMouse : IAction
    {
        public int X { get; set; }

        public int Y { get; set; }

        public MouseClickTypes MouseClickType { get; set; }

        public InputMouse(MouseClickTypes presstype, int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.MouseClickType = presstype;
        }

        public void Do()
        {
            // MOUSEEVENTF_MOVE일 때만 X, Y값이 동작합니다.
            WinAPI.mouse_event((uint)MouseClickType, (uint)X, (uint)Y, 0, 0);
        }
    }
}
