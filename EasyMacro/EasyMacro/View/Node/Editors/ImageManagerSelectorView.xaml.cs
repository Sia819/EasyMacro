using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using EasyMacro.ViewModel;
using EasyMacro.ViewModel.Node.Editors;
using ReactiveUI;

namespace EasyMacro.View.Node.Editors
{
    /// <summary>
    /// ImageManagerSelectorView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ImageManagerSelectorView : UserControl, IViewFor<ImageManagerSelectorViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel),
                                        typeof(ImageManagerSelectorViewModel),
                                        typeof(ImageManagerSelectorView),
                                        new PropertyMetadata(null));

        public ImageManagerSelectorViewModel ViewModel
        {
            get => (ImageManagerSelectorViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ImageManagerSelectorViewModel)value;
        }
        #endregion

        public ImageManagerSelectorView()
        {
            InitializeComponent();

            this.DataContext = ImageManagerViewModel.Instance; 
            this.ViewModel = new ImageManagerSelectorViewModel();

            this.WhenActivated(d =>
            {
                // ImageManagerViewModel 에 있는 Dictionary에 바인딩합니다.
                this.OneWayBind(ImageManagerViewModel.Instance, dc => dc.RegisterdImages, v => v.imageSelector.ItemsSource)
                    .DisposeWith(d);
                this.Bind(ViewModel, vm => vm.SelectedIndex, v => v.imageSelector.SelectedIndex);
            });
        }

        /// <summary> 사용자가 ComboBox의 아이템을 변경함. </summary>
        private void imageSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = sender as ComboBox;
            ImageManagerViewModel.ImageList item = combo.SelectedValue as ImageManagerViewModel.ImageList;
            if (item != null)
            {
                string selectedImageName = item.Name; // Selected ComboBox item string
                if (selectedImageName is null)
                    throw new Exception("ComboBox에서 선택된 string은 null");


                if (this.ViewModel.SelectedBitmap is not null) this.ViewModel.SelectedBitmap.Dispose(); // 이미지 교체전, 이전 이미지가 존재시 메모리해제
                this.ViewModel.SelectedBitmap = ImageManagerViewModel.Instance.RegisterdImages.Find(selectedImageName).ImageClone(); // 이미지를 클론함.

                this.ViewModel.Value = selectedImageName; // 이후 Value-Changed 옵저버가 반응하여, 비트맵을 TemplateMatch객체에 전달함.
            }
        }
    }
}
