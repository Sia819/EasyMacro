﻿using DynamicData;
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
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class CombInputKeyboardViewModel : CodeGenNodeViewModel, INodeSerializable, IExtendedXmlCustomSerializer
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

        public override NodeOutputViewModel GetOutputViewModel => this.FlowIn;

        Action Func()
        {
            Action action = () =>
            {
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { this.IsSelected = true; }));
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
                    combInputKeyboard.AddList(Input.Value);
                    combInputKeyboard.Do();

                    foreach (var a in FlowOut.Values.Items)
                    {
                        a.Compile(new CompilerContext());
                    }
                }
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { this.IsSelected = false; }));
            };
            return action;
        }

        public override void Serializer(XmlWriter xmlWriter, object obj)
        {
            NodeSerializer.SerializerOfNodeViewModel(ref xmlWriter, ref obj);

            CombInputKeyboardViewModel instance = obj as CombInputKeyboardViewModel;

            xmlWriter.WriteElementString(nameof(Input), instance.Input.Value.ToString());
            xmlWriter.WriteElementString(nameof(Alt), instance.Alt.Value.ToString());
            xmlWriter.WriteElementString(nameof(Ctrl), instance.Ctrl.Value.ToString());
            xmlWriter.WriteElementString(nameof(Shift), instance.Shift.Value.ToString());

            int count = 0;
            foreach (var i in instance.FlowOut.Connections.Items)
            {
                xmlWriter.WriteElementString($"ConnedtedHashs_{count}", (i.Output.Parent as CodeGenNodeViewModel).Hash);
                count++;
            }
        }

        public override object Deserialize(XElement xElement)
        {
            CombInputKeyboardViewModel instance = (CombInputKeyboardViewModel)NodeSerializer.DeserializeOfNoveViewModel(ref xElement, this);

            Dictionary<string, XElement> dictionary = NodeSerializer.XElementToDictionary(xElement);
            (instance.Input.Editor as KeyboardRecordEditorViewModel).Value = ((Keys)Enum.Parse(typeof(Keys), dictionary["Input"].Value));
            (instance.Input.Editor as KeyboardRecordEditorViewModel).ReactiveObject.MyKey = ((Keys)Enum.Parse(typeof(Keys), dictionary["Input"].Value)).ToString();
            (instance.Alt.Editor as CheckBoxEditorViewModel).Value = bool.TryParse(dictionary["Alt"].Value, out bool Alt) ? Alt : false;
            (instance.Ctrl.Editor as CheckBoxEditorViewModel).Value = bool.TryParse(dictionary["Ctrl"].Value, out bool Ctrl) ? Ctrl : false;
            (instance.Shift.Editor as CheckBoxEditorViewModel).Value = bool.TryParse(dictionary["Shift"].Value, out bool Shift) ? Shift : false;

            bool isLast = false;
            for (int count = 0; isLast == false; count++)
            {
                if (dictionary.TryGetValue($"ConnedtedHashs_{count}", out XElement element))
                {
                    instance.ConnedtedHashs.Add(element.Value);
                }
                else
                {
                    isLast = true;
                }
            }
            return instance;
        }

        public override void Connect(INodeSerializable instance, List<INodeSerializable> obj)
        {
            foreach (var hashs in this.ConnedtedHashs)
            {
                foreach (CodeGenNodeViewModel allNodes in obj)
                {
                    if (allNodes.Hash == hashs)
                    {
                        allNodes.Parent.Connections.Add(new ConnectionViewModel(this.Parent, this.FlowOut, allNodes.GetOutputViewModel));
                    }
                }
            }
        }

        public CombInputKeyboardViewModel() : base(NodeType.Function)
        {
            base.Name = "조합 키 입력";

            Input = new ValueNodeInputViewModel<Keys>()
            {
                Name = "입력 키",
                Editor = new KeyboardRecordEditorViewModel(),
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

            this.Hash = Common.HashGen.RandomHashGen(10);
        }

        public CombInputKeyboardViewModel(string hash) : this()
        {
            this.Hash = hash;
        }
    }
}
