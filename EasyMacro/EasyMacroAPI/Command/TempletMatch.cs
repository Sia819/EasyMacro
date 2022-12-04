using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Versioning;
using EasyMacroAPI.Model;
using OpenCvSharp.Extensions;

namespace EasyMacroAPI.Command
{
    [SupportedOSPlatform("Windows")]
    public class TempletMatch : IAction
    {
        public static System.Drawing.Point Match(Bitmap screenImg, Bitmap targetImg, double accuracy)
        {
            using (var screenMat = screenImg.ToMat())   // 원본 이미지
            using (var targetMat = targetImg.ToMat())  // 찾을 이미지
            using (var resultMat = screenMat.MatchTemplate(targetMat, OpenCvSharp.TemplateMatchModes.CCoeffNormed))
            {
                double minval, maxval = 0;
                OpenCvSharp.Cv2.MinMaxLoc(resultMat, out minval, out maxval, out OpenCvSharp.Point minloc, out OpenCvSharp.Point maxloc);

                return (maxval >= accuracy) ? new System.Drawing.Point(maxloc.X, maxloc.Y) :
                                              System.Drawing.Point.Empty;
            }
        }

        public double Accuracy { get; set; } = 0.8;
        public System.Drawing.Point FoundPoint { get; private set; }
        public bool IsWantKeepFinding { get; set; } = false;
        public bool Result { get; private set; }
        public int RetryTimes { get; set; } = 0;
        public ScreenCapture ScreenCapture { get; set; }
        public Bitmap TargetImg { get; set; }

        public delegate void Delegate(List<IAction> list);
        
        private Delay delay = new Delay(1000);
        

        public TempletMatch()
        {
            ScreenCapture = new ScreenCapture(IntPtr.Zero);
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
            int count = 0;
            do
            {
                ScreenCapture.Do();
                if (ScreenCapture.CapturedImage is not null)
                {// 창을 찾은 경우
                    FoundPoint = Match(ScreenCapture.CapturedImage, TargetImg, Accuracy);
                    if (FoundPoint != System.Drawing.Point.Empty)
                    {
                        Result = true;
                        break;
                    }
                    else if (FoundPoint == System.Drawing.Point.Empty && !IsWantKeepFinding)
                    {
                        Result = false;
                    }
                }
                else if(ScreenCapture.CapturedImage is null && !IsWantKeepFinding)
                {// 창을 찾지 못한 경우
                    Result = false;
                }
                count++;
            } while (IsWantKeepFinding || RetryTimes > count);
        }

    }
}