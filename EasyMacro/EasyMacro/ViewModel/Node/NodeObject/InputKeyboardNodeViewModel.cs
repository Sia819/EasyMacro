using DynamicData;
using EasyMacro.Model.Node;
using EasyMacro.Model.Node.Compiler;
using EasyMacro.View.Node;
using EasyMacro.ViewModel.Node.Editors;
using EasyMacroAPI.Command;
using EasyMacroAPI.Model;
using ExtendedXmlSerializer;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using NodeNetwork.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using static EasyMacro.ViewModel.Node.Editors.RadioButtonEditorViewModel;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class InputKeyboardNodeViewModel : CodeGenNodeViewModel, INodeSerializable, IExtendedXmlCustomSerializer
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

        public ValueNodeInputViewModel<Keys> PressKey { get; }

        public ValueNodeOutputViewModel<IStatement> FlowIn { get; }

        public ValueListNodeInputViewModel<IStatement> FlowOut { get; }

        

        public bool IsCanExcute { get; set; } = true;

        Action Func()
        {
            Action action = () =>
            {
                if (CodeSimViewModel.Instance.IsRunning || Thread.CurrentThread.IsBackground is false)
                {
                    CodeSimViewModel.Instance.Print((FlowIn.CurrentValue as NodeCompile).CurrentValue);

                    inputKeyboard.Key = PressKey.Value;

                    // RadioButtoKeyboardPressTypen Index to MouseClickTypes Convert
                    switch ((this.KeyboardPressType.Editor as RadioButtonEditorViewModel).RadioSelectedIndex)
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

        public override void Serializer(XmlWriter xmlWriter, object obj)
        {
            NodeSerializer.SerializerOfNodeViewModel(ref xmlWriter, ref obj);

            InputKeyboardNodeViewModel instance = obj as InputKeyboardNodeViewModel;

            int selectedNum;

            switch ((this.KeyboardPressType.Editor as RadioButtonEditorViewModel).RadioSelectedIndex)
            {
                case 0: selectedNum = 0; break;
                case 1: selectedNum = 1; break;

                default: selectedNum = 0; break; // RadioButton에서 아무것도 선택되지 않음.
            }

            xmlWriter.WriteElementString(nameof(KeyboardPressType), selectedNum.ToString());
            xmlWriter.WriteElementString(nameof(PressKey), instance.PressKey.Value.ToString());
        }

        public override object Deserialize(XElement xElement)
        {
            InputKeyboardNodeViewModel instance = (InputKeyboardNodeViewModel)NodeSerializer.DeserializeOfNoveViewModel(ref xElement, new InputKeyboardNodeViewModel());
            Dictionary<string, XElement> dictionary = NodeSerializer.XElementToDictionary(xElement);
            (instance.KeyboardPressType.Editor as RadioButtonEditorViewModel).MyList[int.TryParse(dictionary["KeyboardPressType"].Value, out int KeyboardPressType) ? KeyboardPressType : 0].IsChecked = true;
            (instance.PressKey.Editor as KeyboardRecordEditorViewModel).Value = ((Keys)Enum.Parse(typeof(Keys), dictionary["PressKey"].Value));
            (instance.PressKey.Editor as KeyboardRecordEditorViewModel).ReactiveObject.MyKey = ((Keys)Enum.Parse(typeof(Keys), dictionary["PressKey"].Value)).ToString();
            return instance;
        }

        public override void Connect()
        {
            throw new NotImplementedException();
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
            PressKey = new ValueNodeInputViewModel<Keys>()
            {
                Name = "입력 키",
                Editor = new KeyboardRecordEditorViewModel(),
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

            this.Hash = Common.HashGen.RandomHashGen(10);
        }

        public InputKeyboardNodeViewModel(string hash) : base(NodeType.Function)
        {
            this.Hash = hash;
        }
    }
}
