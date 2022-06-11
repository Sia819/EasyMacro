using EasyMacroAPI.Model;
using System;
using System.Threading;

namespace EasyMacroAPI.Command
{
    public class Delay : IAction
    {
        /// <summary>
        /// 현재 매크로의 타입
        /// </summary>
        public MacroTypes MacroType => MacroTypes.Delay;

        /// <summary>
        /// 대기할 시간
        /// </summary>
        public int Time { get; set; }

        private ManualResetEvent mre;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="time"></param>
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
