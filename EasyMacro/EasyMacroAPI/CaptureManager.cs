using System.Drawing;
using System.Runtime.Versioning;

namespace EasyMacroAPI
{
    [SupportedOSPlatform("Windows")]
    class CaptureManager
    {
        public void ScreenCapture()
        {
            Bitmap bmp = new Bitmap(500, 
                                    500,
                                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics gr = Graphics.FromImage(bmp);

            gr.CopyFromScreen(0, 0, 0, 0, bmp.Size);
        }
    }
}
