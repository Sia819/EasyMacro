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

        #region TargetWindowTitle Property
        public string TargetWindowTitle
        {
            get => targetWindowTitle;
            set => this.RaiseAndSetIfChanged(ref targetWindowTitle, value);
        }
        private string targetWindowTitle;
        #endregion

        #region TargetWindowClass Property
        public string TargetWindowClass
        {
            get => targetWindowClass;
            set => this.RaiseAndSetIfChanged(ref targetWindowClass, value);
        }
        private string targetWindowClass;
        #endregion

        public FindWindowEditorViewModel()
        {
            Value = IntPtr.Zero;
        }
    }
}
