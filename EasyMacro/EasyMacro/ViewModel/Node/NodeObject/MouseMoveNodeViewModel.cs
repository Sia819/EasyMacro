using DynamicData;
using EasyMacro.Model.Node;
using EasyMacro.Model.Node.Compiler;
using EasyMacro.View.Node;
using EasyMacro.ViewModel.Node.Editors;
using EasyMacroAPI.Command;
using MoonSharp.Interpreter;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using NodeNetwork.Views;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class MouseMoveNodeViewModel : CodeGenNodeViewModel
    {
        static MouseMoveNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<MouseMoveNodeViewModel>));
        }

        static MouseMove _mouseMove;
        MouseMove mouseMove 
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

        public ValueNodeInputViewModel<int?> Y { get; }

        public ValueNodeOutputViewModel<IStatement> FlowIn { get; }

        public ValueListNodeInputViewModel<IStatement> FlowOut { get; }

        public ValueNodeInputViewModel<int?> RunButton { get; }

        public bool IsCanExcute { get; set; } = true;

        public MouseMoveNodeViewModel() : base(NodeType.Function)
        {
            this.RunButton = new ValueNodeInputViewModel<int?>()
            {
                Port = null,
                Name = "Run",
                Editor = new RunButtonViewModel()
                {
                    RunScript = ReactiveCommand.Create
                    (
                        () =>
                        {
                            mouseMove.X = (int)X.Value;
                            mouseMove.Y = (int)Y.Value;
                            mouseMove.Do();
                        },
                        this.WhenAnyValue(vm => vm.IsCanExcute)
                    )
                }
            };
            this.Inputs.Add(this.RunButton);

            base.Name = "MouseMove";
            //TODO : 나머지 구현
            X = new ValueNodeInputViewModel<int?>()
            {
                Name = "X",
                Editor = new IntegerValueEditorViewModel(),
            };
            this.Inputs.Add(X);

            Y = new ValueNodeInputViewModel<int?>()
            {
                Name = "Y",
                Editor = new IntegerValueEditorViewModel(),
            };
            this.Inputs.Add(Y);

            // Append Flow
            FlowIn = new CodeGenOutputViewModel<IStatement>(PortType.Execution)
            {
                Name = "Append",
            };
            this.Outputs.Add(FlowIn);

            // Send Flow
            FlowOut = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "Send",
            };
            this.Inputs.Add(FlowOut);


            
        }
    }
}
