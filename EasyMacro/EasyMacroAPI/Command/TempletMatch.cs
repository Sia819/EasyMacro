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

        public Bitmap TargetImg { get; set; }
        public ScreenCapture screenCapture { get; set; }
        private Point point;
        public bool isWantKeepFinding = false;
        public bool result;
        public int retryTimes;
        public Delay delay = new Delay(1000);

        public delegate void Delegate(List<IAction> list);

        public TempletMatch()
        {
            screenCapture = new ScreenCapture();
        }

        ~TempletMatch()
        {
            TargetImg.Dispose();
        }
        /*
        public TempletMatch(string targetDir)
        {
            TargetImg = new Bitmap(targetDir);
            screenCapture = new ScreenCapture();
            folderA = new Folder(this);
            folderB = new Folder();
        }

        public TempletMatch(Bitmap targetImg)
        {
            this.TargetImg = targetImg;
            screenCapture = new ScreenCapture();
            folderA = new Folder(this);
            folderB = new Folder();
        }

        public TempletMatch(string targetDir, string windowName)
        {
            TargetImg = new Bitmap(targetDir);
            screenCapture = new ScreenCapture(windowName);
            folderA = new Folder(this);
            folderB = new Folder();
        }

        public TempletMatch(Bitmap targetImg, string windowName)
        {
            this.TargetImg = targetImg;
            screenCapture = new ScreenCapture(windowName);
            folderA = new Folder(this);
            folderB = new Folder();
        }
        */

        public void SetDelayTime(int time)
        {
            delay.Time = time;
        }

        public void Do()
        {
            int count = 0;
            do
            {
                count++;
                screenCapture.Do();
                if (screenCapture.CapturedImage is not null)
                {// 창을 찾은 경우
                    point = CaptureManager.Instance.TempletMatch(screenCapture.CapturedImage, TargetImg);
                    if (point != Point.Empty)
                    {
                        result = true;
                        break;
                    }
                    else if (point == Point.Empty && !isWantKeepFinding)
                    {
                        result = false;
                    }
                }
                else if(screenCapture.CapturedImage is null && !isWantKeepFinding)
                {// 창을 찾지 못한 경우
                    result = false;
                }
                delay.Do();
            } while (isWantKeepFinding || retryTimes >= count);
        }
    }
}
