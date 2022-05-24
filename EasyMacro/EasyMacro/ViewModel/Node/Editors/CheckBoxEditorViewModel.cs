using EasyMacro.View.Node.Editors;
using NodeNetwork.Toolkit.ValueNode;
using ReactiveUI;

namespace EasyMacro.ViewModel.Node.Editors
{
    public class CheckBoxEditorViewModel : ValueEditorViewModel<bool?>
    {
        static CheckBoxEditorViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CheckBoxEditorView(), typeof(IViewFor<CheckBoxEditorViewModel>));
        }

        public string CheckBoxContent { get; }
        public CheckBoxEditorViewModel(string CheckBoxContent = "")
        {
            Value = false;
            this.CheckBoxContent = CheckBoxContent;
        }

    }
}
