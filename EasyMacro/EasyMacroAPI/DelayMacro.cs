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
            get { return MacroTypes.MouseMove; }
        }

        Timer timer;

        int time;

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
