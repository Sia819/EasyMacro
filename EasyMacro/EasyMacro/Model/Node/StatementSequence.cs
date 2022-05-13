using System.Collections.Generic;
using EasyMacro.Model.Node.Compiler;

namespace EasyMacro.Model.Node
{
    public class StatementSequence : IStatement
    {
        public List<IStatement> Statements { get; } = new List<IStatement>();

        public StatementSequence()
        { }

        public StatementSequence(IEnumerable<IStatement> statements)
        {
            Statements.AddRange(statements);
        }

        public string Compile(CompilerContext context)
        {
            string result = "";
            foreach (IStatement statement in Statements)
            {
                result += statement.Compile(context);
                result += "\n";
            }
            return result;
        }
    }
}
