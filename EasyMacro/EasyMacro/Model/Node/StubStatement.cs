using EasyMacro.Model.Node.Compiler;

namespace EasyMacro.Model.Node
{
    public class StubStatement : IStatement
    {
        public string Compile(CompilerContext context)
        {
            return "";
        }
    }
}
