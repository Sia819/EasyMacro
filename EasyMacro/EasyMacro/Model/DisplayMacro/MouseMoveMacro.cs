using EasyMacroAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMacro.Model.DisplayMacro
{
    public class MouseMacro : IMacros
    {
        public MacroDisplayType DisplayType { get; }

        public string DisplayText
        {
            get
            {
                if (DisplayType == MacroDisplayType.MouseMove)
                    return "마우스 움직임 매크로";
                else if (DisplayType == MacroDisplayType.MouseClick)
                    return "마우스 클릭 매크로";
                else
                    throw new Exception("Unexpected DisplayType saved!!");
            }
        }

        public int X
        {
            get
            {
                if (DisplayType == MacroDisplayType.MouseMove)
                {
                    return (macro as EasyMacroAPI.Command.MouseMove).X;
                }
                else if (DisplayType == MacroDisplayType.MouseClick)
                {
                    return (macro as EasyMacroAPI.Command.MouseClick).X;
                }
                else
                    throw new Exception("Unexpected DisplayType saved!!");
            }
        }

        public int Y
        {
            get
            {
                if (DisplayType == MacroDisplayType.MouseMove)
                {
                    return (macro as EasyMacroAPI.Command.MouseMove).Y;
                }
                else if (DisplayType == MacroDisplayType.MouseClick)
                {
                    return (macro as EasyMacroAPI.Command.MouseClick).Y;
                }
                else
                    throw new Exception("Unexpected DisplayType saved!!");
            }
        }

        public MouseMacro(IAction action)
        {
            if (action is EasyMacroAPI.Command.MouseMove mouseMoveMacro)
            {
                this.macro = mouseMoveMacro;
                DisplayType = MacroDisplayType.MouseMove;
            }
            else if (action is EasyMacroAPI.Command.MouseClick mouseClickMacro)
            {
                this.macro = mouseClickMacro;
                DisplayType = MacroDisplayType.MouseClick;
            }
            else
            {
                throw new Exception("Unexpected type \"" + action.GetType() + "\" Detected!!");
            }
        }

        private EasyMacroAPI.Model.IAction macro;
    }
}
