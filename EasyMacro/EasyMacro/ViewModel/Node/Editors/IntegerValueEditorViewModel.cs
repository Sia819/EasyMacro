using EasyMacro.View.Node.Editors;
using NodeNetwork.Toolkit.ValueNode;
using ReactiveUI;

namespace EasyMacro.ViewModel.Node.Editors

{
    public class IntegerValueEditorViewModel : ValueEditorViewModel<int?>
    {
        static IntegerValueEditorViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new IntegerValueEditorView(), typeof(IViewFor<IntegerValueEditorViewModel>));
        }

        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        public IntegerValueEditorViewModel(int minValue = -2147483648, int maxValue = 2147483647)
        {
            Value = 0;
            this.MinValue = (int)minValue;
            this.MaxValue = (int)maxValue;
        }

    }
}
