using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMacroAPI.Model
{
    /// <summary>
    /// 매크로의 타입을 나타냅니다.<para/>
    /// 주로 IAction의 패러미터로 사용됩니다.
    /// </summary>
    public enum MacroTypes
    {
        Delay = 0,
        MouseMove = 1,
        MouseClick = 2,
        InputKeyboard = 3,
    }
}
