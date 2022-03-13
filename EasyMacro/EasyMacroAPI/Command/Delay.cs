using EasyMacroAPI.Model;

namespace EasyMacroAPI.Command
{
    public class Delay : IAction
    {
        public MacroTypes MacroType
        {
            get { return MacroTypes.Delay; }
        }

        private int time;
        public int Time { get { return time; } set { time = value; } }

        public Delay(int time)
        {
            this.time = time;
        }

        public void Do()
        {
            MacroManager.Instance.DelayMacro(time);
        }
    }
}
