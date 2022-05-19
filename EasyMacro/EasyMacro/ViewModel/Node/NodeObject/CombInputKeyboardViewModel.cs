using DynamicData;
using EasyMacro.Model.Node;
using EasyMacro.Model.Node.Compiler;
using EasyMacro.View.Node;
using EasyMacro.ViewModel.Node.Editors;
using EasyMacroAPI.Command;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using NodeNetwork.Views;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class CombInputKeyboardViewModel : CodeGenNodeViewModel
    {
        static CombInputKeyboardViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<DelayNodeViewModel>));
        }


        /// <summary>
        /// Delay Time
        /// </summary>
        public ValueNodeInputViewModel<int?> Input { get; }

        public ValueNodeOutputViewModel<IStatement> FlowIn { get; }

        public ValueListNodeInputViewModel<IStatement> FlowOut { get; }

        //public ValueNodeInputViewModel<int?> RunButton { get; }

        public bool IsCanExcute { get; set; } = true;

        public CombInputKeyboardViewModel() : base(NodeType.Function)
        {

            base.Name = "Sleep";
            //TODO : 나머지 구현
            Input = new ValueNodeInputViewModel<int?>()
            {
                Name = "Delay Time",
                Editor = new IntegerValueEditorViewModel(),
                Port = null,
            };
            this.Inputs.Add(Input);

            FlowIn = new CodeGenOutputViewModel<IStatement>(PortType.Execution)
            {
                Name = "",
            };
            this.Outputs.Add(FlowIn);

            FlowOut = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "",
            };
            this.Inputs.Add(FlowOut);
            
            
            
        }
    }
}
