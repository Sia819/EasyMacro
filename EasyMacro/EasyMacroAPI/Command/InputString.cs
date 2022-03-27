using System.Threading;
using System.Windows.Forms;
using EasyMacroAPI.Model;

namespace EasyMacroAPI.Command
{
    /// <summary>
    /// summary
    /// </summary>
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
            Thread.Sleep(2000);
            SendKeys.SendWait("123");
        }

        public void Do()
        {
            Change(Text);
        }
    }
}