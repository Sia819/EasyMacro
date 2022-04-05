using System;
using System.Drawing;
using System.Runtime.Versioning;
using EasyMacroAPI.Model;

namespace EasyMacroAPI.Command
{
    [SupportedOSPlatform("Windows")]
    class TempletMatch : IAction
    {
        public MacroTypes MacroType => MacroTypes.TempletMatch;

        private MacroManager macroManager = MacroManager.Instance;

        private bool isMatchWithScreen;
        private Bitmap targetImg;
        private Point point;

        public TempletMatch(string targetDir, bool isMatchWithScreen)
        {
            targetImg = new Bitmap(targetDir);
            this.isMatchWithScreen = isMatchWithScreen;
        }

        public TempletMatch(Bitmap targetImg, bool isMatchWithScreen)
        {
            this.targetImg = targetImg;
            this.isMatchWithScreen = isMatchWithScreen;
        }

        public void Do()
        {
            if(isMatchWithScreen)
                point = CaptureManager.Instance.TempletMatch(macroManager.screenImg, targetImg);
            else
                point = CaptureManager.Instance.TempletMatch(macroManager.windowImg, targetImg);

            if (point != Point.Empty)
            {
                macroManager.tempPoint = point;
            }
        }
    }
}
