using EasyMacro.Model.Node.Compiler;

namespace EasyMacro.Model.Node
{
    public class StringLiteral : ITypedExpression<string>
    {
        public string Value { get; set; }

        public string Compile(CompilerContext ctx)
        {
            return $"\"{Value}\"";
        }
    }
}
