using System;
using System.Threading;
using EasyMacroAPI.Model;

namespace EasyMacroAPI.Command
{
    public class Delay : IAction
    {
        public int Time { get; set; }

        private ManualResetEvent mre;

        public Delay(int time)
        {
            this.Time = time;
        }

        public void Do()
        {
            mre = new ManualResetEvent(false);
            var signalled = mre.WaitOne(TimeSpan.FromMilliseconds(Time));
            
            if (!signalled)
            {
                // Set 호출로 인한 강제 타임아웃 당한 경우.
            }
            mre.Dispose();
        }
    }
}
