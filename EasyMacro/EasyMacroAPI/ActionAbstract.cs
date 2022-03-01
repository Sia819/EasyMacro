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

        int x, y;
        Action action;
        float time;

        public void Do()
        {
            action.Invoke();
        }
    }
}
