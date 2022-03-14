using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace EasyMacroAPI
{
    class CaptureManager
    {
        public void ScreenCapture()
        {
            Bitmap bmp = new Bitmap(500, 500,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics gr = Graphics.FromImage(bmp);

            gr.CopyFromScreen(0, 0, 0, 0, bmp.Size);
        }
    }
}
