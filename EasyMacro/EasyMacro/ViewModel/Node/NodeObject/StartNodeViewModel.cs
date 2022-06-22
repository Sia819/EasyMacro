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

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class StartNodeViewModel : CodeGenNodeViewModel, INodeSerializable, IExtendedXmlCustomSerializer
    {
        static StartNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<StartNodeViewModel>));
        }

        public ValueListNodeInputViewModel<IStatement> OnClickFlow { get; }

        public override void Serializer(XmlWriter xmlWriter, object obj)
        {
            NodeSerializer.SerializerOfNodeViewModel(ref xmlWriter, ref obj);
        }

        public override object Deserialize(XElement xElement)
        {
            StartNodeViewModel instance = (StartNodeViewModel)NodeSerializer.DeserializeOfNoveViewModel(ref xElement, new StartNodeViewModel());
            return instance;
        }

        public override void Connect()
        {
            throw new NotImplementedException();
        }

        public StartNodeViewModel() : base(NodeType.EventNode)
        {
            this.Name = "시작";

            OnClickFlow = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "Start",
                MaxConnections = 1
            };

            this.Inputs.Add(OnClickFlow);

            this.Hash = Common.HashGen.RandomHashGen(10);
        }

        public StartNodeViewModel(string hash) : this()
        {
            this.Hash = hash;
        }

    }
}
