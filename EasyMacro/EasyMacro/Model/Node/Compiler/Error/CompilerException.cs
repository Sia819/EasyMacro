using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMacro.Model.Node.Compiler.Error
{
    public class CompilerException : Exception
    {
        public CompilerException(string msg) : base(msg)
        { }

        public CompilerException(string msg, Exception inner) : base(msg, inner)
        { }
    }
}
