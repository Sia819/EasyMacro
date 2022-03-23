using EasyMacroAPI.Model;

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
            MacroManager.Instance.DelayMacro(Time);
        }
    }
}
