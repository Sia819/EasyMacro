using EasyMacroAPI.Common;
using System;
using System.Drawing;
using System.Runtime.Versioning;

namespace EasyMacroAPI
{
    [SupportedOSPlatform("Windows")]
    public static class CaptureManager
    {
        public static Bitmap WindowCapture(IntPtr hWnd)
        {
            if (hWnd != IntPtr.Zero)
            {
                Bitmap result;
                using (Graphics graphicsData = Graphics.FromHwnd(hWnd))// 찾은 플레이어를 바탕으로 Graphics 정보를 가져옵니다.
                {
                    Rectangle rect = Rectangle.Round(graphicsData.VisibleClipBounds);   // 찾은 플레이어 창 크기 및 위치를 가져옵니다. 
                    result = new Bitmap(rect.Width, rect.Height);                   // 플레이어 창 크기 만큼의 비트맵을 선언해줍니다.
                    using (Graphics g = Graphics.FromImage(result))                        // 비트맵을 바탕으로 그래픽스 함수로 선언해줍니다.
                    {
                        IntPtr hdc = g.GetHdc();
                        WinAPI.PrintWindow(hWnd, hdc, 0x2);
                        g.ReleaseHdc(hdc);
                    }
                }
                return result;
            }
            else
            {
                //플레이어를 못찾을경우
                return null;
            }
        }

        public static Bitmap FullScreenCapture()
        {
            var screens = System.Windows.Forms.Screen.AllScreens;
            var start = new System.Drawing.Point();
            var end = new System.Drawing.Point();
            for (int i = 0; i < screens.Length; i++)
            {
                if (start.X > screens[i].Bounds.Left) start.X = screens[i].Bounds.Left;
                if (start.Y > screens[i].Bounds.Top) start.Y = screens[i].Bounds.Top;
                if (end.X < screens[i].Bounds.Right) end.X = screens[i].Bounds.Width;
                if (end.Y < screens[i].Bounds.Bottom) end.Y = screens[i].Bounds.Height;
            }

            Bitmap bmp = new Bitmap(end.X - start.X,
                                    end.Y - start.Y,
                                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics gr = Graphics.FromImage(bmp))
            {
                gr.CopyFromScreen(start.X, start.Y, 0, 0, bmp.Size);
            }
            return bmp;
        }

        public static Bitmap[] ScreenCapture()
        {
            var screens = System.Windows.Forms.Screen.AllScreens;
            var results = new Bitmap[screens.Length];
            for (int i = 0; i < screens.Length; i++)
            {
                results[i] = new Bitmap(screens[i].Bounds.Width,
                                        screens[i].Bounds.Height,
                                        System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics gr = Graphics.FromImage(results[i]))
                {
                    gr.CopyFromScreen(screens[i].Bounds.Left, screens[i].Bounds.Top, 0, 0, results[i].Size);
                }
            }
            return results;
        }

    }
}
