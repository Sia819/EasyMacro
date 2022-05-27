using System.Linq;
using System.Reactive.Linq;
using DynamicData;
using EasyMacro.Model.Node;
using EasyMacro.Model.Node.Compiler;
using NodeNetwork.Toolkit.ValueNode;
using EasyMacro.View.Node;
using ReactiveUI;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class PrintNode : CodeGenNodeViewModel
    {
        static PrintNode()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<PrintNode>));
        }

        public ValueNodeInputViewModel<ITypedExpression<string>> Text { get; }

        public ValueNodeOutputViewModel<IStatement> Flow { get; }

        public PrintNode() : base(NodeType.Function)
        {
            this.Name = "Print";

            Text = new CodeGenInputViewModel<ITypedExpression<string>>(PortType.String)
            {
                Name = "Text"
            };
            this.Inputs.Add(Text);

            Flow = new CodeGenOutputViewModel<IStatement>(PortType.Execution)
            {
                Name = "",
                Value = this.Text.ValueChanged.Select(stringExpr => new FunctionCall
                {
                    FunctionName = "print",
                    Parameters =
                    {
                        stringExpr ?? new StringLiteral{Value = ""}
                    }
                })
            };
            this.Outputs.Add(Flow);
        }
    }
}
