using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyMacroAPI
{
    public class DelayMacro : IAction
    {
        public MacroTypes MacroType
        {
            get { return MacroTypes.Delay; }
        }

        private int time;
        public int Time { get { return time; } set { time = value; } }

        public DelayMacro(int time)
        {
            this.time = time;
        }

        public void Do()
        {
            MacroManager.Instance.DelayMacro(time);
        }
    }
}
