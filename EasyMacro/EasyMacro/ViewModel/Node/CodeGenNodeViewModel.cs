using EasyMacro.Model.Node;
using EasyMacro.View.Node;
using EasyMacro.ViewModel.Node.NodeObject;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using NodeNetwork.ViewModels;
using ReactiveUI;
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

        public NodeType NodeType { get; }

        public CodeGenNodeViewModel(NodeType type)
        {
            NodeType = type;
        }

        public CodeGenNodeViewModel()
        {
            NodeType = NodeType.NotSupport;
        }

        public abstract void Serializer(XmlWriter xmlWriter, object instance);

        public abstract object Deserialize(XElement xElement);

        public abstract void Connect();
    }
}
