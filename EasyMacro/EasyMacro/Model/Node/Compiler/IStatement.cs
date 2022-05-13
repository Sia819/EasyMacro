namespace EasyMacro.Model.Node.Compiler
{
    public interface IStatement
    {
        string Compile(CompilerContext context);
    }
}