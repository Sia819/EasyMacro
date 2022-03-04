using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMacroAPI
{
    [Serializable]
    public class Hello : ActionAbstract
    {
        private string data;
        public Hello()
        {
            this.data = "Hello";
        }
        public Hello(string data)
        {
            this.data = data;
        }

        public override void Do()
        {
            Console.WriteLine(data);
        }

        public void Edit(string editText)
        {
            this.data = editText;
        }
    }
}
