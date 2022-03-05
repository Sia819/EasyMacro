using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyMacroAPI;

namespace EasyMacro.Model
{

    public class UndefinedMacro : IMacros
    {
        private IAction action;
        public string Text => "지정되지 않은 매크로";
        public bool IsSleep => false;

        public UndefinedMacro(IAction action)
        {
            this.action = action;
        }
    }
}
