using EasyMacro.Model.Node;
using EasyMacro.View.Node;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using NodeNetwork.ViewModels;
using ReactiveUI;
using System.Xml;
using System.Xml.Linq;

namespace EasyMacro.ViewModel.Node
{
    public class CodeGenNodeViewModel : NodeViewModel
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
        
    }
}
