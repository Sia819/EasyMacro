using EasyMacroAPI.Common;
using EasyMacroAPI.Model;

namespace EasyMacroAPI.Command
{
    public class InputKeyboard : IAction
    {
        #region Public Propertes

        // TODO : Windows Forms 라이브러리 키 열거형만 쓸거면 나중에, Model폴더 내 Keys 열거형을 따로 선언하는 방법이 선호됨.
        public Keys Key { get; set; }

        public KeyPressTypes KeyPressTypes { get; set; }

        public MacroTypes MacroType => MacroTypes.InputKeyboard;

        #endregion

        public InputKeyboard() {  }

        public InputKeyboard(Keys key, KeyPressTypes presstype)
        {
            this.Key = key;
            this.KeyPressTypes = presstype;
        }

        public void Do()
        {
            WinAPI.keybd_event((byte)Key, 0, (int)KeyPressTypes, 0);
        }
    }
}
