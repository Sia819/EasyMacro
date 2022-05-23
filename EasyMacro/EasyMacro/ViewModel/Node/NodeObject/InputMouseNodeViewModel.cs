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
    public class InputMouseNodeViewModel : CodeGenNodeViewModel
    {
        static InputMouseNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<InputMouseNodeViewModel>));
        }

        private static InputMouse _inputMouse;
        private InputMouse inputMouse
        {
            get
            {
                if (_inputMouse is null)
                    _inputMouse = new InputMouse(EasyMacroAPI.Model.MouseClickTypes.LBDOWN, 0, 0);
                return _inputMouse;
            }
        }

        public ValueNodeInputViewModel<int?> MouseClickType { get; } // RadioButton

        /// <summary>
        /// Delay Time
        /// </summary>
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

                inputMouse.X = (int)X.Value;
                inputMouse.Y = (int)Y.Value;

                // RadioButton Index to MouseClickTypes Convert
                switch ((this.MouseClickType.Editor as RadioButtonEditorViewModel).GetRadioSelectedIndex)
                {
                    case 0: inputMouse.MouseClickType = EasyMacroAPI.Model.MouseClickTypes.LBDOWN; break;
                    case 1: inputMouse.MouseClickType = EasyMacroAPI.Model.MouseClickTypes.LBUP; break;
                    case 2: inputMouse.MouseClickType = EasyMacroAPI.Model.MouseClickTypes.RBDOWN; break;
                    case 3: inputMouse.MouseClickType = EasyMacroAPI.Model.MouseClickTypes.RBUP; break;

                    default: inputMouse.MouseClickType = EasyMacroAPI.Model.MouseClickTypes.LBDOWN; break; // RadioButton에서 아무것도 선택되지 않음.
                }

                inputMouse.Do();

                foreach (var a in FlowOut.Values.Items)
                {
                    a.Compile(new CompilerContext());
                }
            };
            return action;
        }

        public InputMouseNodeViewModel() : base(NodeType.Function)
        {
            MouseClickType = new ValueNodeInputViewModel<int?>()
            {
                Port = null,
                Name = "MouseClickTypes",
                Editor = new RadioButtonEditorViewModel()
            };
            var temp = (MouseClickType.Editor as RadioButtonEditorViewModel);
            temp.MyList.Add(new MyListItem(true, "LBDOWN", temp.RadioGroupInstanceHash));
            temp.MyList.Add(new MyListItem(false, "LBUP", temp.RadioGroupInstanceHash));
            temp.MyList.Add(new MyListItem(false, "RBDOWN", temp.RadioGroupInstanceHash));
            temp.MyList.Add(new MyListItem(false, "RBUP", temp.RadioGroupInstanceHash));
            this.Inputs.Add(MouseClickType);

            base.Name = "InputMouse";

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
                    Log = Observable.Merge(X.ValueChanged.Select(x => $"InputMouse - ({x.Value}, {this.Y.Value}, {inputMouse.MouseClickType})"),
                                            Y.ValueChanged.Select(y => $"InputMouse - ({this.X.Value}, {y.Value}, {inputMouse.MouseClickType})"),
                                            MouseClickType.ValueChanged.Select(mouseClickTypes => $"InputMouse - ({this.X.Value}, {this.Y.Value}, {inputMouse.MouseClickType})"))
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
