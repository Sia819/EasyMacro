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

        #region SelectedIndex Property
        public int SelectedIndex
        {
            get => selectedIndex;
            set => this.RaiseAndSetIfChanged(ref selectedIndex, value);
        }
        private int selectedIndex;
        #endregion

        public bool CanUseFile { get; set; }

        public void SelectName(string name)
        {
            int count = 0;
            CanUseFile = false;
            foreach (var i in ImageManagerViewModel.Instance.RegisterdImages)
            {
                if (i.Name == name)
                {
                    this.SelectedIndex = count;
                    CanUseFile = true;
                }
                count++;
            }
        }

        public ImageManagerSelectorViewModel()
        {
            Value = "";
        }
    }
}
