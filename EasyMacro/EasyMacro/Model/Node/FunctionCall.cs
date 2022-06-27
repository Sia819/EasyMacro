namespace EasyMacro.Model.Node
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EasyMacro.Model.Node.Compiler;

    public class FunctionCall : IStatement
    {
        public string FunctionName { get; set; }
        public List<IExpression> Parameters { get; } = new List<IExpression>();

        public string Compile(CompilerContext context)
        {
            return $"{FunctionName}({String.Join(", ", Parameters.Select(p => p.Compile(context)))})\n";
        }
    }
}