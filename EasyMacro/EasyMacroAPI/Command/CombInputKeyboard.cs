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

        // TODO : Windows Forms 라이브러리 키 열거형만 쓸거면 나중에, Model폴더 내 Keys 열거형을 따로 선언하는 방법이 선호됨.
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
        /*
        public CombInputKeyboard(Keys[] keys)
        {
            this.Keys = keys;
        }
        */
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
