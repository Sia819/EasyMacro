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

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class MouseClickNodeViewModel : CodeGenNodeViewModel
    {
        static MouseClickNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<MouseClickNodeViewModel>));
        }

        private static MouseClick _mouseClick;
        private MouseClick mouseClick
        {
            get
            {
                if (_mouseClick is null)
                    _mouseClick = new MouseClick(0, 0);
                return _mouseClick;
            }
        }

        /// <summary>
        /// Delay Time
        /// </summary>
        public ValueNodeInputViewModel<int?> X { get; }

        public IntegerValueEditorViewModel XValueEditor = new IntegerValueEditorViewModel();

        public ValueNodeInputViewModel<int?> Y { get; }

        public IntegerValueEditorViewModel YValueEditor = new IntegerValueEditorViewModel();

        public ValueNodeInputViewModel<Point> Point { get; }

        public ValueNodeInputViewModel<int?> Delay { get; }

        public ValueNodeOutputViewModel<IStatement> FlowIn { get; }

        public ValueListNodeInputViewModel<IStatement> FlowOut { get; }

        public ValueNodeInputViewModel<int?> RunButton { get; }

        public bool IsCanExcute { get; set; } = true;

        Action Func()
        {
            Action action = () =>
            {
                CodeSimViewModel.Instance.Print((FlowIn.CurrentValue as NodeCompile).CurrentValue);

                mouseClick.X = (int)X.Value;
                mouseClick.Y = (int)Y.Value;
                mouseClick.Delay = (int)Delay.Value;
                mouseClick.Do();

                foreach (var a in FlowOut.Values.Items)
                {
                    a.Compile(new CompilerContext());
                }
            };
            return action;
        }

        public MouseClickNodeViewModel() : base(NodeType.Function)
        {
            base.Name = "MouseClick";

            X = new ValueNodeInputViewModel<int?>()
            {
                Name = "X",
                Editor = XValueEditor,
                Port = null
            };
            this.Inputs.Add(X);

            Y = new ValueNodeInputViewModel<int?>()
            {
                Name = "Y",
                Editor = YValueEditor,
                Port = null
            };
            this.Inputs.Add(Y);

            Point = new ValueNodeInputViewModel<Point>()
            {
                Name = "Point",
                Editor = new PointRecordEditorViewModel(XValueEditor, YValueEditor),
                Port = null
            };
            this.Inputs.Add(Point);

            Delay = new ValueNodeInputViewModel<int?>()
            {
                Name = "Delay",
                Editor = new IntegerValueEditorViewModel(0) { Value = 40 },
                Port = null,
            };
            this.Inputs.Add(Delay);

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
                    Log = Observable.Merge(X.ValueChanged.Select(x => $"MouseMove - ({x.Value}, {this.Y.Value}, {this.Delay.Value})"),
                                            Y.ValueChanged.Select(y => $"MouseMove - ({this.X.Value}, {y.Value}, , {this.Delay.Value})"),
                                            Delay.ValueChanged.Select(delay => $"MouseMove - ({this.X.Value}, {this.Y.Value}, , {delay ?? 0})"))
                })
            };
            this.Outputs.Add(FlowIn);

            FlowOut = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "Send",
            };
            this.Inputs.Add(FlowOut);
        }
    }
}
