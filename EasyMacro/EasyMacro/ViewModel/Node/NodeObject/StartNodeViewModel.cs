using EasyMacro.Model.Node;
using EasyMacro.Model.Node.Compiler;
using DynamicData;
using NodeNetwork.Toolkit.ValueNode;
using EasyMacro.View.Node;
using ReactiveUI;
using NodeNetwork.ViewModels;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using System.Xml.Linq;
using System.Xml;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class StartNodeViewModel : CodeGenNodeViewModel, INodeSerializable, IExtendedXmlCustomSerializer
    {
        static StartNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<StartNodeViewModel>));
        }

        public ValueListNodeInputViewModel<IStatement> FlowOut { get; }

        public override NodeOutputViewModel GetOutputViewModel => null;

        public override void Serializer(XmlWriter xmlWriter, object obj)
        {
            NodeSerializer.SerializerOfNodeViewModel(ref xmlWriter, ref obj);
            StartNodeViewModel instance = obj as StartNodeViewModel;

            int count = 0;
            foreach (var i in instance.FlowOut.Connections.Items)
            {
                xmlWriter.WriteElementString($"ConnedtedHashs_{count}", (i.Output.Parent as CodeGenNodeViewModel).Hash);
                count++;
            }
        }

        public override object Deserialize(XElement xElement)
        {
            StartNodeViewModel instance = (StartNodeViewModel)NodeSerializer.DeserializeOfNoveViewModel(ref xElement, this);
            Dictionary<string, XElement> dictionary = NodeSerializer.XElementToDictionary(xElement);

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

        public StartNodeViewModel() : base(NodeType.EventNode)
        {
            this.Name = "시작";

            FlowOut = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "Start",
                MaxConnections = 1
            };

            this.Inputs.Add(FlowOut);

            if (PageViewModel.IsLoading is false)
                this.Hash = Common.HashGen.RandomHashGen(10);
        }

        public StartNodeViewModel(string hash) : this()
        {
            this.Hash = hash;
            CanBeRemovedByUser = false;
        }

    }
}
