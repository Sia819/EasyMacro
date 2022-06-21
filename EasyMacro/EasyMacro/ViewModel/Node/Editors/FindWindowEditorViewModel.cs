using System;
using EasyMacro.View.Node.Editors;
using NodeNetwork.Toolkit.ValueNode;
using ReactiveUI;

namespace EasyMacro.ViewModel.Node.Editors
{
    public class FindWindowEditorViewModel : ValueEditorViewModel<IntPtr>
    {
        static FindWindowEditorViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new FindWindowEditorView(), typeof(IViewFor<FindWindowEditorViewModel>));
        }

        public FindWindowEditorViewModel()
        {
            Value = IntPtr.Zero;
        }
    }
}
