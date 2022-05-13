using NodeNetwork.ViewModels;
using ReactiveUI;

namespace EasyMacro.ViewModel.Node
{
    public class CodeGenPortViewModel : PortViewModel
    {
        //static CodeGenPortViewModel()
        //{
        //    Splat.Locator.CurrentMutable.Register(() => new Views.CodeGenPortView(), typeof(IViewFor<CodeGenPortViewModel>));
        //}

        private EasyMacro.Model.Node.PortType _portType;

        public EasyMacro.Model.Node.PortType PortType
        {
            get => _portType;
            set => this.RaiseAndSetIfChanged(ref _portType, value);
        }
    }
}
