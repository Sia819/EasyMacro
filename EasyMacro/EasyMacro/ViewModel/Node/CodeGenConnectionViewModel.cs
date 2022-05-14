using EasyMacro.View.Node;
using NodeNetwork.ViewModels;
using ReactiveUI;

namespace EasyMacro.ViewModel.Node
{
    public class CodeGenConnectionViewModel : ConnectionViewModel
    {
        static CodeGenConnectionViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenConnectionView(), typeof(IViewFor<CodeGenConnectionViewModel>));
        }

        public CodeGenConnectionViewModel(NetworkViewModel parent, NodeInputViewModel input, NodeOutputViewModel output) : base(parent, input, output)
        {

        }
    }
}
