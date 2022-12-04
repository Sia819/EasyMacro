namespace EasyMacro.Model.Node.Compiler
{
    public interface IExpression
    {
        string Compile(CompilerContext context);
    }
}
