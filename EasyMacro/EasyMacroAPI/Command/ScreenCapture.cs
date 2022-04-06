using System.Drawing;
using System.Runtime.Versioning;
using EasyMacroAPI.Model;

namespace EasyMacroAPI.Command
{
    [SupportedOSPlatform("Windows")]
    public class ScreenCapture : IAction
    {
        public MacroTypes MacroType => MacroTypes.ScreenCapture;

        public string WindowName { get; set; }

        public bool IsFullScreen => (WindowName is null) ? true : false;

        public Bitmap CapturedImage { get; private set; }

        public ScreenCapture(string windowName = null)
        {
            if (windowName is not null)
            {
                this.WindowName = windowName;
            }
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

            if (IsFullScreen)
            {
                CapturedImage = CaptureManager.Instance.ScreenCapture();
            }
            else
            {
                CapturedImage = CaptureManager.Instance.WindowCapture(WindowName);
            }

        }
    }
}
