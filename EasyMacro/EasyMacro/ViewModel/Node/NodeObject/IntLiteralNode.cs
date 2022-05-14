using System.Linq;
using System.Reactive.Linq;
using EasyMacro.Model.Node;
using EasyMacro.Model.Node.Compiler;
using EasyMacro.ViewModel.Node.Editors;
using DynamicData;
using NodeNetwork.Toolkit.ValueNode;
using EasyMacro.View.Node;
using ReactiveUI;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class IntLiteralNode : CodeGenNodeViewModel
    {
        static IntLiteralNode()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<IntLiteralNode>));
        }

        public IntegerValueEditorViewModel ValueEditor { get; } = new IntegerValueEditorViewModel();

        public ValueNodeOutputViewModel<ITypedExpression<int>> Output { get; }

        public IntLiteralNode() : base(NodeType.Literal)
        {
            this.Name = "Integer";

            Output = new CodeGenOutputViewModel<ITypedExpression<int>>(PortType.Integer)
            {
                Editor = ValueEditor,
                Value = ValueEditor.ValueChanged.Select(v => new IntLiteral { Value = v ?? 0 })
            };
            this.Outputs.Add(Output);
        }
    }
}
