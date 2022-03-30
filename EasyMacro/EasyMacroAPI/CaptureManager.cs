using OpenCvSharp;
using System;
using System.Drawing;
using System.Runtime.Versioning;
using EasyMacroAPI.Model;

namespace EasyMacroAPI
{
    [SupportedOSPlatform("Windows")]
    public class CaptureManager : IAction
    {
        public MacroTypes MacroType => MacroTypes.ImgSearch;

        [System.Runtime.InteropServices.DllImport("User32", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        string windowName = "";
        Bitmap targetImg = null;
        Bitmap screenImg = null;

        public CaptureManager(string targetDir)
        {
            //this.windowName = windowName;
            this.targetImg = new Bitmap(targetDir);
        }

        public void FindWindow()
        {
            IntPtr findwindow = FindWindow(null, windowName);
            if (findwindow != IntPtr.Zero)
            {
                //찾은 플레이어를 바탕으로 Graphics 정보를 가져옵니다.
                Graphics Graphicsdata = Graphics.FromHwnd(findwindow);

                //찾은 플레이어 창 크기 및 위치를 가져옵니다. 
                Rectangle rect = Rectangle.Round(Graphicsdata.VisibleClipBounds);

                //플레이어 창 크기 만큼의 비트맵을 선언해줍니다.
                Bitmap bmp = new Bitmap(rect.Width, rect.Height);

                //비트맵을 바탕으로 그래픽스 함수로 선언해줍니다.
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    //찾은 플레이어의 크기만큼 화면을 캡쳐합니다.
                    IntPtr hdc = g.GetHdc();
                    PrintWindow(findwindow, hdc, 0x2);
                    g.ReleaseHdc(hdc);
                }
                screenImg = bmp;
            }
            else
            {
                //플레이어를 못찾을경우
                
            }
        }

        public void ScreenCapture()
        {
            Bitmap bmp = new Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width,
                                    System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height,
                                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics gr = Graphics.FromImage(bmp);

            gr.CopyFromScreen(0, 0, 0, 0, bmp.Size);

            screenImg = bmp;
        }

        public void TempletMatch()
        {
            // 원본 이미지
            using (Mat ScreenMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(screenImg))

            // 찾을 이미지
            using (Mat targetMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(targetImg))
            using (Mat result = ScreenMat.MatchTemplate(targetMat, TemplateMatchModes.CCoeffNormed))
            {
                double minval, maxval = 0;
                OpenCvSharp.Point minloc, maxloc;
                Cv2.MinMaxLoc(result, out minval, out maxval, out minloc, out maxloc);

                if(maxval >= 0.8)
                {
                    new Command.MouseMove(maxloc.X, maxloc.Y).Do();
                }
            }
        }

        public void Do()
        {
            ScreenCapture();
            TempletMatch();
        }
    }
}
