using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMacroAPI
{
    public class ActionAbstract
    {
        private MacroManager macroManager = MacroManager.Instance;

        public int index;
        Action action;

        public void Do()
        {
            action.Invoke();
        }
    }
}
