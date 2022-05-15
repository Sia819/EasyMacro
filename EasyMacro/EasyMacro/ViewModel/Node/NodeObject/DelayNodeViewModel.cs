using DynamicData;
using EasyMacro.Model.Node;
using EasyMacro.Model.Node.Compiler;
using EasyMacro.View.Node;
using EasyMacro.ViewModel.Node.Editors;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using NodeNetwork.Views;
using ReactiveUI;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class DelayNodeViewModel : CodeGenNodeViewModel
    {
        static DelayNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<DelayNodeViewModel>));
        }

        /// <summary>
        /// Delay Time
        /// </summary>
        public ValueNodeInputViewModel<int?> Input { get; }

        public ValueNodeOutputViewModel<IStatement> FlowIn { get; }

        public ValueListNodeInputViewModel<IStatement> FlowOut { get; }

        public DelayNodeViewModel() : base(NodeType.Function)
        {
            base.Name = "Sleep";
//TODO : 나머지 구현
            Input = new ValueNodeInputViewModel<int?>()
            {
                Name = "Delay Time",
                Editor = new IntegerValueEditorViewModel()
            };
            this.Inputs.Add(Input);

            FlowOut = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "",
            };
            this.Inputs.Add(FlowOut);
            
            FlowIn = new CodeGenOutputViewModel<IStatement>(PortType.Execution)
            {
                Name = "",
            };
            this.Outputs.Add(FlowIn);
            
        }
    }
}
