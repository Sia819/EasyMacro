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
        private Folder folderA;
        private Folder folderB;
        public bool isWantKeepFinding = false;
        public Delay delay = new Delay(1000);

        public delegate void Delegate(List<IAction> list);

        public TempletMatch()
        {
            folderA = new Folder(this);
            folderB = new Folder();
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
            do
            {
                screenCapture.Do();
                if (screenCapture.CapturedImage is not null)
                {// 창을 찾은 경우
                    point = CaptureManager.Instance.TempletMatch(screenCapture.CapturedImage, TargetImg);
                    if (point != Point.Empty)
                    {
                        folderA.Do();
                        break;
                    }
                    else if (point == Point.Empty && !isWantKeepFinding)
                    {
                        folderB.Do();
                    }
                }
                else if(screenCapture.CapturedImage is null && !isWantKeepFinding)
                {// 창을 찾지 못한 경우
                    folderB.Do();
                }
                delay.Do();
            } while (isWantKeepFinding);
        }
    }
}
