using System.Runtime.Versioning;
using EasyMacroAPI.Model;

namespace EasyMacroAPI.Command
{
    [SupportedOSPlatform("Windows")]
    class FindWindowCapture : IAction
    {
        public MacroTypes MacroType => MacroTypes.FindWindowCapture;

        private string windowName;

        public FindWindowCapture(string windowName)
        {
            this.windowName = windowName;
        }

        public void Do()
        {
            MacroManager.Instance.windowImg = 
                CaptureManager.Instance.FindWindowCapture(windowName);
        }
    }
}
