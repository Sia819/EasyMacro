using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
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

            
        }
    }
}
