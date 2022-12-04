using EasyMacro.Model.Node;
using EasyMacro.View.Node;
using EasyMacro.ViewModel.Node.NodeObject;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using NodeNetwork.ViewModels;
using ReactiveUI;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace EasyMacro.ViewModel.Node
{
    public abstract class CodeGenNodeViewModel : NodeViewModel, INodeSerializable
    {
        static CodeGenNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<CodeGenNodeViewModel>));
        }

        public string Hash { get; protected set; }

        public List<string> ConnedtedHashs { get; }

        public abstract NodeOutputViewModel GetOutputViewModel { get; }

        public NodeType NodeType { get; }

        public CodeGenNodeViewModel(NodeType type)
        {
            NodeType = type;
            ConnedtedHashs = new List<string>();
        }

        public CodeGenNodeViewModel()
        {
            NodeType = NodeType.NotSupport;
            ConnedtedHashs = new List<string>();
        }

        public abstract void Serializer(XmlWriter xmlWriter, object instance);

        public abstract object Deserialize(XElement xElement);

        public abstract void Connect(INodeSerializable instance, List<INodeSerializable> obj);
    }
}
