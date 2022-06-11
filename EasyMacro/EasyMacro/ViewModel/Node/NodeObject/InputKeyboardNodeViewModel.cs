using DynamicData;
using EasyMacro.Model.Node;
using EasyMacro.Model.Node.Compiler;
using EasyMacro.View.Node;
using EasyMacro.ViewModel.Node.Editors;
using EasyMacroAPI.Command;
using EasyMacroAPI.Model;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using NodeNetwork.Views;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using static EasyMacro.ViewModel.Node.Editors.RadioButtonEditorViewModel;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class InputKeyboardNodeViewModel : CodeGenNodeViewModel
    {
        static InputKeyboardNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<InputKeyboardNodeViewModel>));
        }

        private static InputKeyboard _inputKeyboard;
        private InputKeyboard inputKeyboard
        {
            get
            {
                if (_inputKeyboard is null)
                    _inputKeyboard = new InputKeyboard(EasyMacroAPI.Model.Keys.None, EasyMacroAPI.Model.KeyPressTypes.KEY_DOWN);
                return _inputKeyboard;
            }
        }
        
        public ValueNodeInputViewModel<int?> RunButton { get; }

        public ValueNodeInputViewModel<int?> KeyboardPressType { get; }

        public ValueNodeInputViewModel<string> PressKey { get; }

        public ValueNodeOutputViewModel<IStatement> FlowIn { get; }

        public ValueListNodeInputViewModel<IStatement> FlowOut { get; }

        

        public bool IsCanExcute { get; set; } = true;

        Action Func()
        {
            Action action = () =>
            {
                if (CodeSimViewModel.Instance.IsRunning)
                {
                    CodeSimViewModel.Instance.Print((FlowIn.CurrentValue as NodeCompile).CurrentValue);
                    //TODO : 키 지정방식 수정 필요, 작동 안됨

                    inputKeyboard.Key = (Keys)Enum.Parse(typeof(Keys), PressKey.Value);

                    // RadioButtoKeyboardPressTypen Index to MouseClickTypes Convert
                    switch ((this.KeyboardPressType.Editor as RadioButtonEditorViewModel).GetRadioSelectedIndex)
                    {
                        case 0: inputKeyboard.KeyPressTypes = KeyPressTypes.KEY_DOWN; break;
                        case 1: inputKeyboard.KeyPressTypes = KeyPressTypes.KEY_UP; break;

                        default: inputKeyboard.KeyPressTypes = KeyPressTypes.KEY_DOWN; break; // RadioButton에서 아무것도 선택되지 않음.
                    }

                    inputKeyboard.Do();

                    foreach (var a in FlowOut.Values.Items)
                    {
                        a.Compile(new CompilerContext());
                    }
                }
            };
            return action;
        }

        public InputKeyboardNodeViewModel() : base(NodeType.Function)
        {
            // 노드의 이름
            base.Name = "키보드 버튼 누르고/떼고있기";

            // Key-Down, Key-Up ComboBox Editor 추가
            KeyboardPressType = new ValueNodeInputViewModel<int?>()
            {
                Port = null,
                Name = "누르기/떼기",
                Editor = new RadioButtonEditorViewModel()
            };
            var temp = (KeyboardPressType.Editor as RadioButtonEditorViewModel);
            temp.MyList.Add(new MyListItem(true, "누르고있기", temp.RadioGroupInstanceHash));
            temp.MyList.Add(new MyListItem(false, "떼기", temp.RadioGroupInstanceHash));
            this.Inputs.Add(KeyboardPressType);

            // 입력할 키를 파싱하는 TextBox Editor 추가
            PressKey = new ValueNodeInputViewModel<string>()
            {
                Name = "입력 키",
                Editor = new StringValueEditorViewModel(),
                Port = null
            };
            this.Inputs.Add(PressKey);

            // Run-Button Editor 추가
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

            //TODO : 나머지 구현

            FlowIn = new CodeGenOutputViewModel<IStatement>(PortType.Execution)
            {
                Name = "",
                Value = this.RunButton.ValueChanged.Select(_ => new NodeCompile(this.Func())
                {
                    Log = Observable.Merge(PressKey.ValueChanged.Select(key => $"InputKeyboard - ({key}, {this.inputKeyboard.KeyPressTypes})"),
                                            KeyboardPressType.ValueChanged.Select(Types => $"InputKeyboard - ({this.PressKey.Value}, {Types})"))
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
