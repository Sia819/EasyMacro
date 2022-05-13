using EasyMacro.Model.Node;
using NodeNetwork.ViewModels;

namespace EasyMacro.ViewModel.Node
{
    public class CodeGenNodeViewModel : NodeViewModel
    {
        //static CodeGenNodeViewModel()
        //{
        //    Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<CodeGenNodeViewModel>));
        //}

        public NodeType NodeType { get; }

        public CodeGenNodeViewModel(NodeType type)
        {
            NodeType = type;
        }
    }
}
