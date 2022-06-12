using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyMacroAPI.Model;

namespace EasyMacroAPI.Command
{
    public class CombInputKeyboard : IAction
    {
        #region Public Propertes
        public List<Keys> Keys { get; set; }

        public MacroTypes MacroType => MacroTypes.CombInputKeyboard;

        private InputKeyboard inputKeyboard;
        #endregion

        public CombInputKeyboard()
        {
            Keys = new List<Keys>();
            inputKeyboard = new InputKeyboard();
        }

        public void AddList(Keys key)
        {
            Keys.Add(key);
        }

        public void Do()
        {
            for(int i = 0; i < Keys.Count; i++)
            {
                inputKeyboard.Key = Keys[i];
                inputKeyboard.KeyPressTypes = KeyPressTypes.KEY_DOWN;
                inputKeyboard.Do();
            }

            for(int i = Keys.Count -1; i >= 0; i--)
            {
                inputKeyboard.Key = Keys[i];
                inputKeyboard.KeyPressTypes = KeyPressTypes.KEY_UP;
                inputKeyboard.Do();
            }
        }
    }
}
