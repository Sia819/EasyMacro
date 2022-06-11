using EasyMacro.View.Node.Editors;
using NodeNetwork.Toolkit.ValueNode;
using ReactiveUI;
using System.Drawing;

namespace EasyMacro.ViewModel.Node.Editors
{
    public class ImageManagerSelectorViewModel : ValueEditorViewModel<string>
    {
        static ImageManagerSelectorViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ImageManagerSelectorView(), typeof(IViewFor<ImageManagerSelectorViewModel>));
        }

        public Bitmap SelectedBitmap { get; set; }

        public ImageManagerSelectorViewModel()
        {
            Value = null;
        }
    }
}
