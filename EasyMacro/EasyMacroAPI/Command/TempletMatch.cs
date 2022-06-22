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
        public ScreenCapture ScreenCapture { get; set; }
        public Point FoundPoint { get; private set; }
        public int retryTimes { get; set; } = 0;
        public bool Result { get; private set; }
        public double Accuracy { get; set; } = 0.8;

        
        public bool IsWantKeepFinding { get; set; } = false;
        
        
        
        private Delay delay = new Delay(1000);

        public delegate void Delegate(List<IAction> list);

        public TempletMatch()
        {
            ScreenCapture = new ScreenCapture();
        }

        ~TempletMatch()
        {
            TargetImg.Dispose();
        }
        

        public void SetDelayTime(int time)
        {
            delay.Time = time;
        }

        public void Do()
        {
            int count = -1;
            do
            {
                count++;
                ScreenCapture.Do();
                if (ScreenCapture.CapturedImage is not null)
                {// 창을 찾은 경우
                    FoundPoint = CaptureManager.Instance.TempletMatch(ScreenCapture.CapturedImage, TargetImg, Accuracy);
                    if (FoundPoint != Point.Empty)
                    {
                        Result = true;
                        break;
                    }
                    else if (FoundPoint == Point.Empty && !IsWantKeepFinding)
                    {
                        Result = false;
                    }
                }
                else if(ScreenCapture.CapturedImage is null && !IsWantKeepFinding)
                {// 창을 찾지 못한 경우
                    Result = false;
                }
                if(IsWantKeepFinding || retryTimes > count)
                    delay.Do();
            } while (IsWantKeepFinding || retryTimes > count);
        }
    }
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