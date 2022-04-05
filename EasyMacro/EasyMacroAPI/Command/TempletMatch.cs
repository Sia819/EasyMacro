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

        private string windowName;
        private Bitmap targetImg;
        private Point point;

        public TempletMatch(string targetDir, string windowName)
        {
            targetImg = new Bitmap(targetDir);
            this.windowName = windowName;
        }

        public TempletMatch(Bitmap targetImg, string windowName)
        {
            this.targetImg = targetImg;
            this.windowName = windowName;
        }

        public void Do()
        {
            if(windowName == "")
            {
                point = CaptureManager.Instance.TempletMatch(macroManager.screenImg, targetImg);
            }
            else
            {
                if (macroManager.windowImg.ContainsKey(windowName))
                {
                    point = CaptureManager.Instance.TempletMatch(macroManager.windowImg[windowName], targetImg);
                }
                else
                {
                    // 딕셔너리에서 윈도우 캡쳐 비트맵이 제거된 경우(더 이상 그 윈도우 캡쳐를 원하지 않는 경우)
                }
            }
                

            if (point != Point.Empty)
            {
                macroManager.tempPoint = point;
            }
        }
    }
}
