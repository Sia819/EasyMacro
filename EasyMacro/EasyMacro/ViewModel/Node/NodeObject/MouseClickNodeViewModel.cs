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
    public class MouseClickNodeViewModel : CodeGenNodeViewModel
    {
        static MouseClickNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<MouseClickNodeViewModel>));
        }

        /// <summary>
        /// Delay Time
        /// </summary>
        public ValueNodeInputViewModel<int?> X { get; }

        public ValueNodeInputViewModel<int?> Y { get; }

        public ValueNodeOutputViewModel<IStatement> FlowIn { get; }

        public ValueListNodeInputViewModel<IStatement> FlowOut { get; }

        public MouseClickNodeViewModel() : base(NodeType.Function)
        {
            base.Name = "MouseClick";
//TODO : 나머지 구현
            X = new ValueNodeInputViewModel<int?>()
            {
                Name = "X",
                Editor = new IntegerValueEditorViewModel()
            };
            this.Inputs.Add(X);

            Y = new ValueNodeInputViewModel<int?>()
            {
                Name = "Y",
                Editor = new IntegerValueEditorViewModel()
            };
            this.Inputs.Add(Y);

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
