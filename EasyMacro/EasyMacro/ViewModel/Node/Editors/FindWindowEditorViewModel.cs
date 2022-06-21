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

        public string TargetWindowTitle { get; set; }

        public string TargetWindowClass { get; set; }

        public FindWindowEditorViewModel()
        {
            Value = IntPtr.Zero;
        }
    }
}
