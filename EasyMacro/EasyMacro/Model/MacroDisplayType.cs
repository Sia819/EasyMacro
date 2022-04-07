using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMacro.Model
{
    /// <summary>
    /// 이것이 추가된다면, 아래의 기능 구현부도 추가로 수정되어야 합니다. <para/>
    /// 1. <see cref="EasyMacro.ViewModel.MainWindowViewModel.Add(IMacro)"/> <para/>
    /// 2. <see cref="EasyMacro.UC.PropertiesEditor.PropertiesEditor"/>
    /// </summary>
    public enum MacroDisplayType
    {
        Undefined,
        Sleep,
        MouseMove,
        MouseClick,
    }
}
