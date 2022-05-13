using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyMacro.Model.Node.Compiler;
using EasyMacro.Model.Node.Compiler.Error;

namespace EasyMacro.Model.Node
{
    public class VariableReference<T> : ITypedExpression<T>
    {
        public ITypedVariableDefinition<T> LocalVariable { get; set; }

        public string Compile(CompilerContext context)
        {
            if (!context.IsInScope(LocalVariable))
            {
                throw new VariableOutOfScopeException(LocalVariable.VariableName);
            }
            return LocalVariable.VariableName;
        }
    }
}
