namespace EasyMacro.Model.Node.Compiler
{
    using System.Collections.Generic;

    public class ScopeDefinition
    {
        public string Identifier { get; }

        public List<IVariableDefinition> Variables { get; } = new List<IVariableDefinition>();

        public ScopeDefinition(string identifier)
        {
            Identifier = identifier;
        }
    }
}
