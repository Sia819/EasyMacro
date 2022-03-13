using System.Runtime.InteropServices;
using System.Windows.Forms;
using EasyMacroAPI.Common;
using EasyMacroAPI.Model;

namespace EasyMacroAPI.Command
{
    public class InputKeyboard : IAction
    {
        public MacroTypes MacroType { get { return MacroTypes.InputKeyboard; } }

        private Keys key;
        public Keys Key { get { return key; } set { key = value; } }

        private KeyPressTypes keyPressTypes;
        public KeyPressTypes KeyPressTypes { get { return keyPressTypes; } set { keyPressTypes = value; } }

        

        public InputKeyboard(Keys key, KeyPressTypes presstype)
        {
            this.key = key;
            keyPressTypes = presstype;
        }

        public void Do()
        {
            WinAPI.keybd_event((byte)key, 0, (int)keyPressTypes, 0);
        }
    }
}
