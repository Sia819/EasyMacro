using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyMacro.Model.Node;

namespace EasyMacro.Model.Node.Compiler
{
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
