using EasyMacro.Model.Node;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;

namespace EasyMacro.ViewModel.Node
{
    public class CodeGenListInputViewModel<T> : ValueListNodeInputViewModel<T>
    {
        //static CodeGenListInputViewModel()
        //{
        //    Splat.Locator.CurrentMutable.Register(() => new NodeInputView(), typeof(IViewFor<CodeGenListInputViewModel<T>>));
        //}

        public CodeGenListInputViewModel(PortType type)
        {
            this.Port = new CodeGenPortViewModel { PortType = type };

            if (type == PortType.Execution)
            {
                this.PortPosition = PortPosition.Right;
            }
        }
    }
}
