using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EasyMacroAPI
{
    class InputKeyboard : IAction
    {
        public MacroTypes MacroType { get { return MacroTypes.InputKeyboard; } }

        private Keys key;
        public Keys Key { get { return key; } set { key = value; } }

        private KeyPressTypes keyPressTypes;
        public KeyPressTypes KeyPressTypes { get { return keyPressTypes; } set { keyPressTypes = value; } }

        [DllImport("user32.dll")]
        static extern void keybd_event(byte vk, byte scan, int flags, int extrainfo);

        public InputKeyboard(Keys key, KeyPressTypes presstype)
        {
            this.key = key;
            keyPressTypes = presstype;
        }

        public void Do()
        {
            keybd_event((byte)key, 0, (int)keyPressTypes, 0);
        }
    }
}
