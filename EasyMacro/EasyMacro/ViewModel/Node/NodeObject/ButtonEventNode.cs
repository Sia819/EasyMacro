using EasyMacro.Model.Node;
using EasyMacro.Model.Node.Compiler;
using DynamicData;
using NodeNetwork.Toolkit.ValueNode;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class ButtonEventNode : CodeGenNodeViewModel
    {
        //static ButtonEventNode()
        //{
        //    Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<ButtonEventNode>));
        //}

        public ValueListNodeInputViewModel<IStatement> OnClickFlow { get; }

        public ButtonEventNode() : base(NodeType.EventNode)
        {
            this.Name = "Button Events";

            OnClickFlow = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "On Click"
            };

            this.Inputs.Add(OnClickFlow);
        }
    }
}
