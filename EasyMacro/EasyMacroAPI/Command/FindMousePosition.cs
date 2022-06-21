using System.Runtime.InteropServices;
using EasyMacroAPI.Model;
using System.Windows;
using System.Drawing;

namespace EasyMacroAPI.Command
{
    public class FindMousePosition : IAction
    {
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        public POINT point;

        public int X { get; set; }

        public int Y { get; set; }

        public MacroTypes MacroType => MacroTypes.FindMousePosition;

        private FindWindowPosition findWindowPostion;

        public FindMousePosition(string windowName = "")
        {
            if(windowName != "")
                findWindowPostion = new FindWindowPosition(windowName);
        }

        public void ChangeWindowName(string windowName)
        {
            if(findWindowPostion is null)
            {
                findWindowPostion = new FindWindowPosition(windowName);
            }
            else
            {
                findWindowPostion.WindowName = windowName;
            }
        }

        public void Do()
        {
            if(findWindowPostion.WindowName != "")
            {
                X = 0; Y = 0;
            }
            else
            {
                findWindowPostion.Do();
                X = findWindowPostion.ClientRect.Left;
                Y = findWindowPostion.ClientRect.Top;
            }
            GetCursorPos(out point);
            X = point.X - X;
            Y = point.Y - Y;
        }
    }
}
