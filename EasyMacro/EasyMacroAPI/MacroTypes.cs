using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMacroAPI
{
    public enum MacroTypes
    {
        Hello = -1,
        Sleep = 0,
        MouseMove = 1,
        MouseClick = 2,
        InputKeyboard = 3,
    }

    public enum KeyPressTypes
    {
        KEY_DOWN = 0x00,
        KEY_UP = 0x02
    }
}
