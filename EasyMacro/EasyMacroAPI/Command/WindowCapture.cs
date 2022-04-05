using System.Drawing;
using System.Runtime.Versioning;
using EasyMacroAPI.Model;

namespace EasyMacroAPI.Command
{
    [SupportedOSPlatform("Windows")]
    class WindowCapture : IAction
    {
        public MacroTypes MacroType => MacroTypes.WindowCapture;

        private string windowName;

        public WindowCapture(string windowName)
        {
            this.windowName = windowName;
        }

        public void Do()
        {
            MacroManager.Instance.windowImg.Add(windowName, CaptureManager.Instance.WindowCapture(windowName));
        }
    }
}
