using EasyMacro.Model.Node;
using EasyMacro.Model.Node.Compiler;
using DynamicData;
using NodeNetwork.Toolkit.ValueNode;
using EasyMacro.View.Node;
using ReactiveUI;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class StartNodeViewModel : CodeGenNodeViewModel
    {
        static StartNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<StartNodeViewModel>));
        }

        public ValueListNodeInputViewModel<IStatement> OnClickFlow { get; }

        public StartNodeViewModel() : base(NodeType.EventNode)
        {
            this.Name = "시작";

            OnClickFlow = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "Start"
            };

            this.Inputs.Add(OnClickFlow);
        }
    }
}
