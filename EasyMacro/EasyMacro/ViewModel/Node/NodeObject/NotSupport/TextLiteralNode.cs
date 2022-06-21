using System.Linq;
using System.Reactive.Linq;
using DynamicData;
using EasyMacro.Model.Node;
using EasyMacro.Model.Node.Compiler;
using EasyMacro.ViewModel.Node.Editors;
using NodeNetwork.Toolkit.ValueNode;
using EasyMacro.View.Node;
using ReactiveUI;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    /*
    public class TextLiteralNode : CodeGenNodeViewModel
    {
        static TextLiteralNode()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<TextLiteralNode>));
        }

        public StringValueEditorViewModel ValueEditor { get; } = new StringValueEditorViewModel();

        public ValueNodeOutputViewModel<ITypedExpression<string>> Output { get; }

        public TextLiteralNode() : base(NodeType.Literal)
        {
            this.Name = "Text";

            Output = new CodeGenOutputViewModel<ITypedExpression<string>>(PortType.String)
            {
                Name = "Value",
                Editor = ValueEditor,
                Value = ValueEditor.ValueChanged.Select(v => new StringLiteral { Value = v })
            };
            this.Outputs.Add(Output);
        }
    }
    */
}
