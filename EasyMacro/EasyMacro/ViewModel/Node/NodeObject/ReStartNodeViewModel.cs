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
using System.Windows;
using System.Windows.Threading;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class ReStartNodeViewModel : CodeGenNodeViewModel
    {
        static ReStartNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<ReStartNodeViewModel>));
        }

        /// <summary>
        /// Delay Time
        /// </summary>
        /// 

        public ValueNodeOutputViewModel<IStatement> FlowIn { get; }

        public ValueListNodeInputViewModel<IStatement> FlowOut { get; }

        public ValueNodeInputViewModel<int?> RunButton { get; }

        public bool IsCanExcute { get; set; } = true;
        
        Action Func()
        { //TODO : 스레드로 변경시 수정
            Action action = () =>
            {
                if (CodeSimViewModel.Instance.IsRunning)
                {
                    CodeSimViewModel.Instance.Print((FlowIn.CurrentValue as NodeCompile).CurrentValue);
                    StartNodeViewModel eventNode = PageViewModel.Instance.eventNode;
                    Application.Current.MainWindow.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, new Action(() =>
                    {
                        CodeSimViewModel.Instance.ReStart = true;
                    }
                    ));
                }
            };
            return action;
        }

        public ReStartNodeViewModel() : base(NodeType.EventNode)
        {
            base.Name = "처음으로 돌아가기";

            this.RunButton = new ValueNodeInputViewModel<int?>()
            {
                Port = null,

                Editor = new RunButtonViewModel()
                {
                    RunScript = ReactiveCommand.Create
                    (
                        Func(),
                        this.WhenAnyValue(vm => vm.IsCanExcute)
                    )
                }
            };
            //this.Inputs.Add(this.RunButton);

            FlowIn = new CodeGenOutputViewModel<IStatement>(PortType.Execution)
            {
                Name = "",
                Value = this.RunButton.ValueChanged.Select(_ => new NodeCompile(this.Func())
                {
                    Log = RunButton.ValueChanged.Select(_ => $"!ReStart")
                })
            };
            this.Outputs.Add(FlowIn);

            FlowOut = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "",
            };
            //this.Inputs.Add(FlowOut);
        }
    }
}
