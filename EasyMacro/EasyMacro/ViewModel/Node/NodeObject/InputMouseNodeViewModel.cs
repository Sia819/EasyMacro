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
                }
            };
            return action;
        }

        public InputMouseNodeViewModel() : base(NodeType.Function)
        {
            base.Name = "마우스 버튼 누르고/떼고있기";

            MouseClickType = new ValueNodeInputViewModel<int?>()
            {
                Port = null,
                Name = "누르기/떼기",
                Editor = new RadioButtonEditorViewModel()
            };
            var temp = (MouseClickType.Editor as RadioButtonEditorViewModel);
            temp.MyList.Add(new MyListItem(true, "왼쪽 버튼 누르고 있기", temp.RadioGroupInstanceHash));
            temp.MyList.Add(new MyListItem(false, "왼쪽 버튼 떼기", temp.RadioGroupInstanceHash));
            temp.MyList.Add(new MyListItem(false, "오른쪽 버튼 누르고 있기", temp.RadioGroupInstanceHash));
            temp.MyList.Add(new MyListItem(false, "오른쪽 버튼 떼기", temp.RadioGroupInstanceHash));
            this.Inputs.Add(MouseClickType);

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
                    Log = MouseClickType.ValueChanged.Select(mouseClickTypes => $"InputMouse - ({inputMouse.MouseClickType})")
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
