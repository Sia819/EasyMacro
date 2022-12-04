using System.Drawing;
using System.Runtime.InteropServices;
using EasyMacroAPI.Model;
using static EasyMacroAPI.Common.WinAPI;

namespace EasyMacroAPI.Command
{
    internal class FindMousePosition : IAction
    {
        public POINT point;

        public int X { get; set; }

        public int Y { get; set; }

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

