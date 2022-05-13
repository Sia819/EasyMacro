using EasyMacro.Model.Node.Compiler;

namespace EasyMacro.Model.Node
{
    public class IntLiteral : ITypedExpression<int>
    {
        public int Value { get; set; }

        public string Compile(CompilerContext context)
        {
            return Value.ToString();
        }
    }
}