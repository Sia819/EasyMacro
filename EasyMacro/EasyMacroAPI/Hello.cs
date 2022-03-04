using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMacroAPI
{
    /// <summary>
    /// 임시 매크로 동작 예시입니다.
    /// </summary>
    [Serializable]
    public class Hello : IAction
    {
        public MacroTypes MacroType => MacroTypes.Sleep;

        private string data;

        public Hello()
        {
            this.data = "Hello";
        }
        public Hello(string data)
        {
            this.data = data;
        }

        public void Do()
        {
            Console.WriteLine(data);
        }

        public void Edit(string editText)
        {
            this.data = editText;
        }
    }
}