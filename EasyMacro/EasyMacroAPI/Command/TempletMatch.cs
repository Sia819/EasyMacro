using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Versioning;
using EasyMacroAPI.Model;

namespace EasyMacroAPI.Command
{
    [SupportedOSPlatform("Windows")]
    public class TempletMatch : IAction
    {
        public MacroTypes MacroType => MacroTypes.TempletMatch;

        private Bitmap targetImg;
        private ScreenCapture screenCapture;
        private Point point;

        public delegate void Delegate(List<IAction> list);

        public TempletMatch(string targetDir)
        {
            targetImg = new Bitmap(targetDir);
            screenCapture = new ScreenCapture();
        }

        public TempletMatch(Bitmap targetImg)
        {
            this.targetImg = targetImg;
            screenCapture = new ScreenCapture();
        }

        public TempletMatch(string targetDir, string windowName)
        {
            targetImg = new Bitmap(targetDir);
            screenCapture = new ScreenCapture(windowName);
        }

        public TempletMatch(Bitmap targetImg, string windowName)
        {
            this.targetImg = targetImg;
            screenCapture = new ScreenCapture(windowName);
        }

        public void Do()
        {
            screenCapture.Do();
            if (screenCapture.CapturedImage is not null)
            {// 창을 찾은 경우
                point = CaptureManager.Instance.TempletMatch(screenCapture.CapturedImage, targetImg);

                if (point != Point.Empty)
                {
                    MacroManager.Instance.tempPoint = point;
                }
            }
            else
            {// 창을 찾지 못한 경우
                
            }
            
        }
    }
}
