using System;
using System.Reactive.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using EasyMacro.Model.Node;
using EasyMacro.Model.Node.Compiler;
using EasyMacro.View.Node;
using EasyMacro.ViewModel.Node.Editors;
using EasyMacroAPI.Command;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using NodeNetwork.Views;
using DynamicData;
using ReactiveUI;
using ExtendedXmlSerializer;
using ExtendedXmlSerializer.ExtensionModel.Xml;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class DelayNodeViewModel : CodeGenNodeViewModel, INodeSerializable, IExtendedXmlCustomSerializer
    {
        static DelayNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<DelayNodeViewModel>));
        }

        private static Delay _delay;
        private Delay delay
        {
            get
            {
                if (_delay is null)
                    _delay = new Delay(1000);
                return _delay;
            }
        }

        /// <summary>
        /// Delay Time
        /// </summary>
        public ValueNodeInputViewModel<int?> Delay { get; }

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

                    delay.Time = (int)Delay.Value < 0 ? 0 : (int)Delay.Value;
                    delay.Do();

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
            DelayNodeViewModel instance = obj as DelayNodeViewModel;
            xmlWriter.WriteElementString(nameof(Delay), instance.Delay.Value.ToString());
        }

        public override object Deserialize(XElement xElement)
        {
            DelayNodeViewModel instance = (DelayNodeViewModel)NodeSerializer.DeserializeOfNoveViewModel(ref xElement, new DelayNodeViewModel());
            
            Dictionary<string, XElement> dictionary = NodeSerializer.XElementToDictionary(xElement);
            (instance.Delay.Editor as IntegerValueEditorViewModel).Value = int.TryParse(dictionary["Delay"].Value, out int delay) ? delay : 0;
            return instance;
        }


        public DelayNodeViewModel() : base(NodeType.Function)
        {
            base.Name = "지연시키기";

            Delay = new ValueNodeInputViewModel<int?>()
            {
                Name = "지연시간 (1초 = 1000)",
                Editor = new IntegerValueEditorViewModel(0) { Value = 1000 },
                Port = null,
            };
            this.Inputs.Add(Delay);

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
                    Log = Delay.ValueChanged.Select(input => $"Delay - {input ?? 0}")
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
