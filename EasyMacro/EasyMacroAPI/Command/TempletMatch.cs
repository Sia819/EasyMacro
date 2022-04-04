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

        private Bitmap targetImg;
        private Bitmap screenImg;
        private Point point;

        public TempletMatch(string targetDir)
        {
            targetImg = new Bitmap(targetDir);
        }

        public TempletMatch(Bitmap targetImg)
        {
            this.targetImg = targetImg;
        }

        public void Do()
        {
            screenImg = CaptureManager.Instance.ScreenCapture();
            point = CaptureManager.Instance.TempletMatch(screenImg, targetImg);
            if(point != Point.Empty)
            {
                MacroManager.Instance.tempPoint = point;
            }
        }
    }
}
