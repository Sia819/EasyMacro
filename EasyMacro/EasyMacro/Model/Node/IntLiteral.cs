namespace EasyMacro.Model.Node
{
    using EasyMacro.Model.Node.Compiler;

    public class IntLiteral : ITypedExpression<int>
    {
        public int Value { get; set; }

        public string Compile(CompilerContext context)
        {
            return Value.ToString();
        }
    }
}