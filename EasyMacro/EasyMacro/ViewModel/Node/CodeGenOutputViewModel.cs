using EasyMacro.Model.Node;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using NodeNetwork.Views;
using ReactiveUI;

namespace EasyMacro.ViewModel.Node
{
    public class CodeGenOutputViewModel<T> : ValueNodeOutputViewModel<T>
    {
        static CodeGenOutputViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new NodeOutputView(), typeof(IViewFor<CodeGenOutputViewModel<T>>));
        }

        public CodeGenOutputViewModel(PortType type)
        {
            this.Port = new CodeGenPortViewModel { PortType = type };

            if (type == PortType.Execution)
            {
                this.PortPosition = PortPosition.Left;
            }
        }
    }
}
