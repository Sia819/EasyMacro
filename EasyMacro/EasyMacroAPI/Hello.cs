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
        public override void Do()
        {
            Console.WriteLine("Hello");
        }
    }
}
