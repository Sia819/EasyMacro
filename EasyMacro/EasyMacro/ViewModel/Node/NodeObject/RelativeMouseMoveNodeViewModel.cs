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
using static EasyMacro.ViewModel.Node.Editors.RadioButtonEditorViewModel;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class RelativeMouseMoveNodeViewModel : CodeGenNodeViewModel
    {
        static RelativeMouseMoveNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<RelativeMouseMoveNodeViewModel>));
        }

        private static RelativeMouseMove _relativeMouseMove;
        private RelativeMouseMove relativeMouseMove
        {
            get
            {
                if (_relativeMouseMove is null)
                    _relativeMouseMove = new RelativeMouseMove("", 0, 0);
                return _relativeMouseMove;
            }
        }

        /// <summary>
        /// Delay Time
        /// </summary>
        /// 
        public ValueNodeInputViewModel<string> WindowName { get; }
        public ValueNodeInputViewModel<int?> X { get; }

        public ValueNodeInputViewModel<int?> Y { get; }

        public ValueNodeOutputViewModel<IStatement> FlowIn { get; }

        public ValueListNodeInputViewModel<IStatement> FlowOut { get; }

        public ValueNodeInputViewModel<int?> RunButton { get; }

        public bool IsCanExcute { get; set; } = true;

        Action Func()
        {
            Action action = () =>
            {
                CodeSimViewModel.Instance.Print((FlowIn.CurrentValue as NodeCompile).CurrentValue);

                relativeMouseMove.ChangeWindowName(WindowName.Value);
                relativeMouseMove.X = (int)X.Value;
                relativeMouseMove.Y = (int)Y.Value;

                relativeMouseMove.Do();

                foreach (var a in FlowOut.Values.Items)
                {
                    a.Compile(new CompilerContext());
                }
            };
            return action;
        }

        public RelativeMouseMoveNodeViewModel() : base(NodeType.Function)
        {
            base.Name = "RelativeMouseMove";

            WindowName = new ValueNodeInputViewModel<string>()
            {
                Port = null,
                Name = "WindowName",
                Editor = new StringValueEditorViewModel()
            };
            this.Inputs.Add(WindowName);

            X = new ValueNodeInputViewModel<int?>()
            {
                Name = "X",
                Editor = new IntegerValueEditorViewModel(),
                Port = null
            };
            this.Inputs.Add(X);

            Y = new ValueNodeInputViewModel<int?>()
            {
                Name = "Y",
                Editor = new IntegerValueEditorViewModel(),
                Port = null
            };
            this.Inputs.Add(Y);

            this.RunButton = new ValueNodeInputViewModel<int?>()
            {
                Port = null,
                Name = "Run",
                Editor = new RunButtonViewModel()
                {
                    RunScript = ReactiveCommand.Create
                    (
                        Func(),
                        this.WhenAnyValue(vm => vm.IsCanExcute)
                    )
                }
            };
            this.Inputs.Add(this.RunButton);

            FlowIn = new CodeGenOutputViewModel<IStatement>(PortType.Execution)
            {
                Name = "",
                Value = this.RunButton.ValueChanged.Select(_ => new NodeCompile(this.Func())
                {
                    Log = Observable.Merge(WindowName.ValueChanged.Select(windowName => $"InputMouse - ({windowName}, {this.X.Value}, {this.Y.Value})"),
                                            X.ValueChanged.Select(x => $"InputMouse - ({WindowName}, {x.Value}, {this.Y.Value})"),
                                            Y.ValueChanged.Select(y => $"InputMouse - ({WindowName}, {this.X.Value}, {y.Value})"))
                })
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
