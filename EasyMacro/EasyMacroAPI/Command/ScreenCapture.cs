using System.Runtime.Versioning;
using EasyMacroAPI.Model;

namespace EasyMacroAPI.Command
{
    [SupportedOSPlatform("Windows")]
    class ScreenCapture : IAction
    {
        public MacroTypes MacroType => MacroTypes.ScreenCapture;

        public void Do()
        {
            MacroManager.Instance.screenImg = CaptureManager.Instance.ScreenCapture();
        }
    }
}
