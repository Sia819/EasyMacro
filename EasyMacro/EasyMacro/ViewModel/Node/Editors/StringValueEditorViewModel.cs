using NodeNetwork.Toolkit.ValueNode;

namespace EasyMacro.ViewModel.Node.Editors
{
    public class StringValueEditorViewModel : ValueEditorViewModel<string>
    {
        //static StringValueEditorViewModel()
        //{
        //    Splat.Locator.CurrentMutable.Register(() => new StringValueEditorView(), typeof(IViewFor<StringValueEditorViewModel>));
        //}

        public StringValueEditorViewModel()
        {
            Value = "";
        }
    }
}
