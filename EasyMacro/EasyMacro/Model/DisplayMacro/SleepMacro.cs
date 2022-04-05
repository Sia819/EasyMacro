using EasyMacroAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMacro.Model.DisplayMacro
{
    public class SleepMacro : IMacro
    {

        public MacroDisplayType DisplayType => MacroDisplayType.Sleep;

        public string DisplayText => "대기";

        public uint Millisecond
        {
            get
            {
                return Convert.ToUInt32((macro as EasyMacroAPI.Command.Delay).Time);
            }
        }

        public SleepMacro(IAction action)
        {
            if (action is EasyMacroAPI.Command.Delay macro)
            {
                this.macro = macro;
            }
            else
            {
                throw new Exception("Unexpected type \"" + action.GetType() + "\" Detected!!");
            }
        }

        EasyMacroAPI.Command.Delay macro;
    }
}
