using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using EasyMacro.Model.Node;
using EasyMacro.Model.Node.Compiler;
using DynamicData;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using EasyMacro.View.Node;
using ReactiveUI;
using EasyMacro.ViewModel.Node.Editors;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using System.Xml;
using System.Xml.Linq;
using ExtendedXmlSerializer;
using System.Collections.Generic;
using System;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class ForLoopNode : CodeGenNodeViewModel, INodeSerializable, IExtendedXmlCustomSerializer
    {
        static ForLoopNode()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<ForLoopNode>));
        }

        public ValueNodeOutputViewModel<IStatement> FlowIn { get; }

        public ValueListNodeInputViewModel<IStatement> LoopBodyFlow { get; }
        public ValueListNodeInputViewModel<IStatement> LoopEndFlow { get; }

        public ValueNodeInputViewModel<ITypedExpression<int>> FirstIndex { get; }
        public ValueNodeInputViewModel<int?> LastIndex { get; }

        public ValueNodeOutputViewModel<ITypedExpression<int>> CurrentIndex { get; }

        public IntegerValueEditorViewModel currentIndexEditor = new IntegerValueEditorViewModel(0);

        public override NodeOutputViewModel GetOutputViewModel => this.FlowIn;

        public List<string> ConnedtedHashs2 = new List<string>();

        public override void Serializer(XmlWriter xmlWriter, object obj)
        {
            NodeSerializer.SerializerOfNodeViewModel(ref xmlWriter, ref obj);

            ForLoopNode instance = obj as ForLoopNode;

            xmlWriter.WriteElementString(nameof(LastIndex), instance.LastIndex.Value.ToString());

            int count = 0;
            foreach (var i in instance.LoopBodyFlow.Connections.Items)
            {
                xmlWriter.WriteElementString($"ConnedtedHashs1_{count}", (i.Output.Parent as CodeGenNodeViewModel).Hash);
                count++;
            }
            count = 0;
            foreach (var i in instance.LoopEndFlow.Connections.Items)
            {
                xmlWriter.WriteElementString($"ConnedtedHashs2_{count}", (i.Output.Parent as CodeGenNodeViewModel).Hash);
                count++;
            }
        }

        public override object Deserialize(XElement xElement)
        {
            ForLoopNode instance = (ForLoopNode)NodeSerializer.DeserializeOfNoveViewModel(ref xElement, this);

            Dictionary<string, XElement> dictionary = NodeSerializer.XElementToDictionary(xElement);
            (instance.LastIndex.Editor as IntegerValueEditorViewModel).Value = int.TryParse(dictionary["LastIndex"].Value, out int LastIndex) ? LastIndex : 1;

            bool isLast = false;
            for (int count = 0; isLast == false; count++)
            {
                if (dictionary.TryGetValue($"ConnedtedHashs1_{count}", out XElement element))
                {
                    instance.ConnedtedHashs.Add(element.Value);
                }
                else
                {
                    isLast = true;
                }
            }
            isLast = false;
            for (int count = 0; isLast == false; count++)
            {
                if (dictionary.TryGetValue($"ConnedtedHashs2_{count}", out XElement element))
                {
                    instance.ConnedtedHashs2.Add(element.Value);
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
                        allNodes.Parent.Connections.Add(new ConnectionViewModel(this.Parent, this.LoopBodyFlow, allNodes.GetOutputViewModel));
                    }
                }
            }

            foreach (var hashs in this.ConnedtedHashs2)
            {
                foreach (CodeGenNodeViewModel allNodes in obj)
                {
                    if (allNodes.Hash == hashs)
                    {
                        allNodes.Parent.Connections.Add(new ConnectionViewModel(this.Parent, this.LoopEndFlow, allNodes.GetOutputViewModel));
                    }
                }
            }
        }

        public ForLoopNode() : base(NodeType.FlowControl)
        {
            var controlFlowGroup = new EndpointGroup("");

            var controlFlowInputsGroup = new EndpointGroup(controlFlowGroup);

            this.Name = "반복";

            LoopBodyFlow = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "반복할 노드들",
                Group = controlFlowInputsGroup,
                MaxConnections = 1,
            };
            this.Inputs.Add(LoopBodyFlow);

            LoopEndFlow = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "반복이 끝난 후",
                Group = controlFlowInputsGroup,
                MaxConnections = 1,
            };
            this.Inputs.Add(LoopEndFlow);

            
            FirstIndex = new CodeGenInputViewModel<ITypedExpression<int>>(PortType.Integer)
            {
                Name = "First Index",
            };
            //this.Inputs.Add(FirstIndex);
            

            LastIndex = new ValueNodeInputViewModel<int?>
            {
                Name = "몇 번 반복할지",
                Editor = new IntegerValueEditorViewModel(1) { Value = 1 },
                Port = null,
            };
            this.Inputs.Add(LastIndex);

            ForLoop value = new ForLoop(currentIndexEditor);

            var loopBodyChanged = LoopBodyFlow.Values.Connect().Select(_ => Unit.Default).StartWith(Unit.Default);
            var loopEndChanged = LoopEndFlow.Values.Connect().Select(_ => Unit.Default).StartWith(Unit.Default);
            FlowIn = new CodeGenOutputViewModel<IStatement>(PortType.Execution)
            {
                Name = "",
                Value = Observable.CombineLatest(loopBodyChanged, loopEndChanged, FirstIndex.ValueChanged, LastIndex.ValueChanged,
                        (bodyChange, endChange, firstI, lastI) => (BodyChange: bodyChange, EndChange: endChange, FirstI: firstI, LastI: lastI))
                    .Select(v => {
                        value.LoopBody = new StatementSequence(LoopBodyFlow.Values.Items);
                        value.LoopEnd = new StatementSequence(LoopEndFlow.Values.Items);
                        value.LowerBound = 0;
                        value.UpperBound = v.LastI.Value;
                        return value;
                    }),
                Group = controlFlowGroup
            };
            this.Outputs.Add(FlowIn);

            CurrentIndex = new CodeGenOutputViewModel<ITypedExpression<int>>(PortType.Integer)
            {
                Name = "몇 번째 반복중인지",
                Value = Observable.Return(new VariableReference<int> { LocalVariable = value.CurrentIndex }),
                Editor = currentIndexEditor,
                Port = null,
            };
            this.Outputs.Add(CurrentIndex);

            this.Hash = Common.HashGen.RandomHashGen(10);
        }

        public ForLoopNode(string hash) : this()
        {
            this.Hash = hash;
        }
    }
}
