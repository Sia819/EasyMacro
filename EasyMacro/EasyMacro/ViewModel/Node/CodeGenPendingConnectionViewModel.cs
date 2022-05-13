using NodeNetwork.ViewModels;

namespace EasyMacro.ViewModel.Node
{
    public class CodeGenPendingConnectionViewModel : PendingConnectionViewModel
    {
        //static CodeGenPendingConnectionViewModel()
        //{
        //    Splat.Locator.CurrentMutable.Register(() => new CodeGenPendingConnectionView(), typeof(IViewFor<CodeGenPendingConnectionViewModel>));
        //}

        public CodeGenPendingConnectionViewModel(NetworkViewModel parent) : base(parent)
        {

        }
    }
}
