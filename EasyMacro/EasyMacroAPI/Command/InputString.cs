using EasyMacroAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


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
            SendKeys.Send("123");
        }

        public void Do()
        {
            Change(Text);
        }
    }
}