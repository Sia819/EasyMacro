using DynamicData;
using EasyMacro.Model.Node;
using EasyMacro.Model.Node.Compiler;
using EasyMacro.View.Node;
using EasyMacro.ViewModel.Node.Editors;
using EasyMacroAPI.Command;
using ExtendedXmlSerializer;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using NodeNetwork.Views;
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
    public class InputStringNodeViewModel : CodeGenNodeViewModel, INodeSerializable, IExtendedXmlCustomSerializer
    {
        static InputStringNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<InputStringNodeViewModel>));
        }

        private static InputString _inputString;
        private InputString inputString
        {
            get
            {
                if (_inputString is null)
                    _inputString = new InputString("");
                return _inputString;
            }
        }

        /// <summary>
        /// Delay Time
        /// </summary>
        public ValueNodeInputViewModel<string> Input { get; }

        public ValueNodeOutputViewModel<IStatement> FlowIn { get; }

        public ValueListNodeInputViewModel<IStatement> FlowOut { get; }

        public ValueNodeInputViewModel<int?> RunButton { get; }

        public override NodeOutputViewModel GetOutputViewModel => this.FlowIn;
        public bool IsCanExcute { get; set; } = true;

        Action Func()
        {
            Action action = () =>
            {
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { this.IsSelected = true; }));
                if (CodeSimViewModel.Instance.IsRunning || Thread.CurrentThread.IsBackground is false)
                {
                    CodeSimViewModel.Instance.Print((FlowIn.CurrentValue as NodeCompile).CurrentValue);

                    inputString.Text = Input.Value;
                    inputString.Do();

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

            InputStringNodeViewModel instance = obj as InputStringNodeViewModel;

            xmlWriter.WriteElementString(nameof(Input), instance.Input.Value.ToString());

            int count = 0;
            foreach (var i in instance.FlowOut.Connections.Items)
            {
                xmlWriter.WriteElementString($"ConnedtedHashs_{count}", (i.Output.Parent as CodeGenNodeViewModel).Hash);
                count++;
            }
        }

        public override object Deserialize(XElement xElement)
        {
            InputStringNodeViewModel instance = (InputStringNodeViewModel)NodeSerializer.DeserializeOfNoveViewModel(ref xElement, this);
            Dictionary<string, XElement> dictionary = NodeSerializer.XElementToDictionary(xElement);
            (instance.Input.Editor as StringValueEditorViewModel).Value = dictionary["Input"].Value;

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

        public InputStringNodeViewModel() : base(NodeType.Function)
        {
            base.Name = "문자열 입력";
            //TODO : 나머지 구현

            Input = new ValueNodeInputViewModel<string>()
            {
                Name = "입력할 문자열",
                Editor = new StringValueEditorViewModel(),
                Port = null
            };
            this.Inputs.Add(Input);

            this.RunButton = new ValueNodeInputViewModel<int?>()
            {
                Port = null,

                Editor = new RunButtonViewModel()
                {
                    RunScript = ReactiveCommand.Create(Func(), this.WhenAnyValue(vm => vm.IsCanExcute) )
                }
            };
            this.Inputs.Add(this.RunButton);

            FlowIn = new CodeGenOutputViewModel<IStatement>(PortType.Execution)
            {
                Name = "",
                Value = this.RunButton.ValueChanged.Select(_ => new NodeCompile(this.Func())
                {
                    Log = Observable.Merge(Input.ValueChanged.Select(stringExpr => $"InputString - ({stringExpr})"))
                })
            };
            this.Outputs.Add(FlowIn);

            FlowOut = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "Send",
                MaxConnections = 1
            };
            this.Inputs.Add(FlowOut);

            this.Hash = Common.HashGen.RandomHashGen(10);
        }

        public InputStringNodeViewModel(string hash) : this()
        {
            this.Hash = hash;
        }
    }
}
