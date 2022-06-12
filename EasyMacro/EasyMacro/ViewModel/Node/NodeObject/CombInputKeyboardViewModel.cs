using DynamicData;
using EasyMacro.Model.Node;
using EasyMacro.Model.Node.Compiler;
using EasyMacro.View.Node;
using EasyMacro.ViewModel.Node.Editors;
using EasyMacroAPI.Command;
using EasyMacroAPI.Model;
using NodeNetwork.Toolkit.ValueNode;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class CombInputKeyboardViewModel : CodeGenNodeViewModel
    {
        static CombInputKeyboardViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<CombInputKeyboardViewModel>));
        }

        private static CombInputKeyboard _combInputKeyboard;
        private CombInputKeyboard combInputKeyboard
        {
            get
            {
                if (_combInputKeyboard is null)
                    _combInputKeyboard = new CombInputKeyboard();
                return _combInputKeyboard;
            }
        }

        /// <summary>
        /// Delay Time
        /// </summary>
        public ValueNodeInputViewModel<Keys> Input { get; }

        public ValueNodeInputViewModel<bool?> Alt { get; }

        public ValueNodeInputViewModel<bool?> Ctrl { get; }

        public ValueNodeInputViewModel<bool?> Shift { get; }

        public ValueNodeOutputViewModel<IStatement> FlowIn { get; }

        public ValueListNodeInputViewModel<IStatement> FlowOut { get; }

        public ValueNodeInputViewModel<int?> RunButton { get; }

        public bool IsCanExcute { get; set; } = true;

        Action Func()
        {
            Action action = () =>
            {
                if (CodeSimViewModel.Instance.IsRunning || Thread.CurrentThread.IsBackground is false)
                {
                    CodeSimViewModel.Instance.Print((FlowIn.CurrentValue as NodeCompile).CurrentValue);
                    combInputKeyboard.Keys = new System.Collections.Generic.List<Keys>();
                    if (Ctrl.Value ?? false)
                    {
                        combInputKeyboard.AddList((Keys)Enum.Parse(typeof(Keys), "ControlKey"));
                    }
                    if (Alt.Value ?? false)
                    {
                        combInputKeyboard.AddList((Keys)Enum.Parse(typeof(Keys), "Menu"));
                    }
                    if (Shift.Value ?? false)
                    {
                        combInputKeyboard.AddList((Keys)Enum.Parse(typeof(Keys), "ShiftKey"));
                    }
                    combInputKeyboard.AddList(Input.Value);//((Keys)Enum.Parse(typeof(Keys), Input.Value));
                    combInputKeyboard.Do();

                    foreach (var a in FlowOut.Values.Items)
                    {
                        a.Compile(new CompilerContext());
                    }
                }
            };
            return action;
        }

        public CombInputKeyboardViewModel() : base(NodeType.Function)
        {
            base.Name = "조합 키 입력";

            //TODO : 나머지 구현
            Input = new ValueNodeInputViewModel<Keys>()
            {
                Name = "입력 키",
                Editor = new KeyboardRecordEditorViewModel(),//new StringValueEditorViewModel(),
                Port = null,
            };
            this.Inputs.Add(Input);

            Alt = new ValueNodeInputViewModel<bool?>()
            {
                Name = null,
                Editor = new CheckBoxEditorViewModel("Alt"),
                Port = null,
            };
            this.Inputs.Add(Alt);

            Ctrl = new ValueNodeInputViewModel<bool?>()
            {
                Name = null,
                Editor = new CheckBoxEditorViewModel("Ctrl"),
                Port = null,
            };
            this.Inputs.Add(Ctrl);

            Shift = new ValueNodeInputViewModel<bool?>()
            {
                Name = null,
                Editor = new CheckBoxEditorViewModel("Shift"),
                Port = null,
            };
            this.Inputs.Add(Shift);

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
                    Log = Observable.Merge(Input.ValueChanged.Select(key => $"CombinputKeyboard - ({this.Alt.Value} + {this.Ctrl.Value} + {this.Shift.Value} + {key})"),
                                            Alt.ValueChanged.Select(Alt => $"CombinputKeyboard - ({Alt} + {this.Ctrl.Value} + {this.Shift.Value} + {this.Input.Value})"),
                                            Ctrl.ValueChanged.Select(Ctrl => $"CombinputKeyboard - ({this.Alt.Value} + {Ctrl} + {this.Shift.Value} + {this.Input.Value})"),
                                            Shift.ValueChanged.Select(Shift => $"CombinputKeyboard - ({this.Alt.Value} + {this.Ctrl.Value} + {Shift} + {this.Input.Value})"))
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
