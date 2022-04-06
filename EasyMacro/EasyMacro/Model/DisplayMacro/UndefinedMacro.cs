using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyMacroAPI;
using EasyMacroAPI.Model;

namespace EasyMacro.Model.DisplayMacro
{

    public class UndefinedMacro : IMacro
    {
        #region Public Properties

        public string DisplayText => "지정되지 않은 매크로";
        public bool IsSleep => false;
        public MacroDisplayType DisplayType => MacroDisplayType.Undefined;

        #endregion

        public UndefinedMacro(IAction action)
        {
            this.action = action;
        }

        private IAction action;
    }
}
