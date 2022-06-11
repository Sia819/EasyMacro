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
using System.Drawing;
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

        public IntegerValueEditorViewModel XValueEditor = new IntegerValueEditorViewModel(0);
        public ValueNodeInputViewModel<int?> Y { get; }

        public IntegerValueEditorViewModel YValueEditor = new IntegerValueEditorViewModel(0);

        public ValueNodeInputViewModel<Point> Point { get; }

        public ValueNodeOutputViewModel<IStatement> FlowIn { get; }

        public ValueListNodeInputViewModel<IStatement> FlowOut { get; }

        public ValueNodeInputViewModel<int?> RunButton { get; }

        public bool IsCanExcute { get; set; } = true;

        Action Func()
        {
            Action action = () =>
            {
                if (CodeSimViewModel.Instance.IsRunning)
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
                }
            };
            return action;
        }

        public RelativeMouseMoveNodeViewModel() : base(NodeType.Function)
        {
            base.Name = "마우스 상대좌표 이동";

            WindowName = new ValueNodeInputViewModel<string>()
            {
                Port = null,
                Name = "프로세스 이름",
                Editor = new StringValueEditorViewModel()
            };
            this.Inputs.Add(WindowName);

            X = new ValueNodeInputViewModel<int?>()
            {
                Name = "X좌표",
                Editor = XValueEditor,
                Port = null
            };
            this.Inputs.Add(X);

            Y = new ValueNodeInputViewModel<int?>()
            {
                Name = "Y좌표",
                Editor = YValueEditor,
                Port = null,
            };
            this.Inputs.Add(Y);

            Point = new ValueNodeInputViewModel<Point>()
            {
                Name = "Point",
                Editor = new PointRecordEditorViewModel(XValueEditor, YValueEditor, WindowName),
                Port = null
            };
            this.Inputs.Add(Point);

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
                MaxConnections = 1
            };
            this.Inputs.Add(FlowOut);
        }
    }
}
