﻿using DynamicData;
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
        public ValueNodeInputViewModel<string> Input { get; }

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
                CodeSimViewModel.Instance.Print((FlowIn.CurrentValue as NodeCompile).CurrentValue);

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
                combInputKeyboard.AddList((Keys)Enum.Parse(typeof(Keys), Input.Value));
                combInputKeyboard.Do();

                foreach (var a in FlowOut.Values.Items)
                {
                    a.Compile(new CompilerContext());
                }
            };
            return action;
        }

        public CombInputKeyboardViewModel() : base(NodeType.Function)
        {

            base.Name = "CombInputKeyboard";
            //TODO : 나머지 구현
            Input = new ValueNodeInputViewModel<string>()
            {
                Name = "Input Key",
                Editor = new StringValueEditorViewModel(),
                Port = null,
            };
            this.Inputs.Add(Input);

            Alt = new ValueNodeInputViewModel<bool?>()
            {
                Name = "Alt",
                Editor = new CheckBoxEditorViewModel(),
                Port = null,
            };
            this.Inputs.Add(Alt);

            Ctrl = new ValueNodeInputViewModel<bool?>()
            {
                Name = "Ctrl",
                Editor = new CheckBoxEditorViewModel(),
                Port = null,
            };
            this.Inputs.Add(Ctrl);

            Shift = new ValueNodeInputViewModel<bool?>()
            {
                Name = "Shift",
                Editor = new CheckBoxEditorViewModel(),
                Port = null,
            };
            this.Inputs.Add(Shift);

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
                    Log = Observable.Merge(Input.ValueChanged.Select(key => $"CombinputKeyboard - ({this.Alt} + {this.Ctrl} + {this.Shift} + {key})"),
                                            Alt.ValueChanged.Select(Alt => $"CombinputKeyboard - ({Alt} + {this.Ctrl} + {this.Shift} + {this.Input})"),
                                            Ctrl.ValueChanged.Select(Ctrl => $"CombinputKeyboard - ({this.Alt} + {Ctrl} + {this.Shift} + {this.Input})"),
                                            Shift.ValueChanged.Select(Shift => $"CombinputKeyboard - ({this.Alt} + {this.Ctrl} + {Shift} + {this.Input})"))
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