using EasyMacro.Model.Node.Compiler;

namespace EasyMacro.Model.Node
{
    public interface IVariableDefinition : IStatement
    {
        string VariableName { get; }
    }
}