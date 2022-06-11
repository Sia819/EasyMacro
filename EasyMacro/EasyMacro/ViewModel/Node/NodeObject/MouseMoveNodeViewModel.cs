﻿using DynamicData;
using EasyMacro.Model.Node;
using EasyMacro.Model.Node.Compiler;
using EasyMacro.View.Node;
using EasyMacro.ViewModel.Node.Editors;
using EasyMacroAPI.Command;
using NodeNetwork.Toolkit.ValueNode;
using ReactiveUI;
using System;
using System.Drawing;
using System.Reactive.Linq;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class MouseMoveNodeViewModel : CodeGenNodeViewModel
    {
        static MouseMoveNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<MouseMoveNodeViewModel>));
        }

        private static MouseMove _mouseMove;
        private MouseMove mouseMove
        {
            get
            {
                if (_mouseMove is null)
                    _mouseMove = new MouseMove(0, 0);
                return _mouseMove;
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

                    mouseMove.X = (int)X.Value;
                    mouseMove.Y = (int)Y.Value;
                    mouseMove.Do();

                    foreach (var a in FlowOut.Values.Items)
                    {
                        a.Compile(new CompilerContext());
                    }
                }
            };
            return action;
        }

        public MouseMoveNodeViewModel() : base(NodeType.Function)
        {
            base.Name = "마우스 절대좌표 이동";
            
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
                    Log = Observable.Merge(X.ValueChanged.Select(x => $"MouseMove - ({x.Value}, {this.Y.Value})"),
                                            Y.ValueChanged.Select(y => $"MouseMove - ({this.X.Value}, {y.Value})"))// 
                })
            };
            this.Outputs.Add(FlowIn);

            // Send Flow, 현재 노드에서 내보낼 (매크로의 실행이 끝났다는) 신호
            FlowOut = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "Send",
            };
            this.Inputs.Add(FlowOut);
        }
    }
}
