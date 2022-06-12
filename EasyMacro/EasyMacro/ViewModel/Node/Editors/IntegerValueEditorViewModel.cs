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

        #region MinValue Property
        public int MinValue
        {
            get => minValue;
            set => this.RaiseAndSetIfChanged(ref minValue, value);
        }
        private int minValue;
        #endregion

        #region MaxValue Property
        public int MaxValue
        {
            get => maxValue;
            set => this.RaiseAndSetIfChanged(ref maxValue, value);
        }
        private int maxValue;
        #endregion

        #region Editable Property
        public bool Editable
        {
            get => editable;
            set => this.RaiseAndSetIfChanged(ref editable, value);
        }
        private bool editable;
        #endregion

        public IntegerValueEditorViewModel(int minValue = -2147483648, int maxValue = 2147483647)
        {
            Value = 0;
            this.MinValue = (int)minValue;
            this.MaxValue = (int)maxValue;
            this.Editable = true;
        }

    }
}
