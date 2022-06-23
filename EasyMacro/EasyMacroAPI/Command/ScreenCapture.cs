using System;
using System.Drawing;
using System.Runtime.Versioning;
using EasyMacroAPI.Model;

namespace EasyMacroAPI.Command
{
    [SupportedOSPlatform("Windows")]
    public class ScreenCapture : IAction
    {
        public MacroTypes MacroType => MacroTypes.ScreenCapture;

        public IntPtr hWnd { get; set; }

        public bool IsWindowCapture { get; set; }

        public Bitmap CapturedImage { get; private set; }

        public ScreenCapture(IntPtr hWnd)
        {
            this.hWnd = hWnd;
        }

        ~ScreenCapture()
        {
            CapturedImage.Dispose();
        }

        public void Do()
        {
            if (CapturedImage is not null)
            {
                CapturedImage.Dispose();
            }

            if (IsWindowCapture)
            {
                CapturedImage = CaptureManager.Instance.WindowCapture(hWnd);
            }
            else
            {
                CapturedImage = CaptureManager.Instance.ScreenCapture();
            }

        }
    }
}
