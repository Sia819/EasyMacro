using EasyMacroAPI.Model;

namespace EasyMacroAPI.Command
{
    public class InputString : IAction
    {
        public MacroTypes MacroType => MacroTypes.InputString;
        public string Text { get; set; }
        public InputString(string data)
        {
            this.Text = data;
        }
        private void Change(string data)
        {
            System.Windows.Forms.SendKeys.SendWait(data);
        }

        public void Do()
        {
            Change(Text);
        }
    }
}