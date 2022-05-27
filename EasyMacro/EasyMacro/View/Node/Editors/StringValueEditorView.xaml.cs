using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using EasyMacro.ViewModel.Node;
using EasyMacro.ViewModel.Node.Editors;
using ReactiveUI;

namespace EasyMacro.View.Node.Editors
{
    /// <summary>
    /// StringValueEditorView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StringValueEditorView : UserControl, IViewFor<StringValueEditorViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(StringValueEditorViewModel), typeof(StringValueEditorView), new PropertyMetadata(null));

        public StringValueEditorViewModel ViewModel
        {
            get => (StringValueEditorViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (StringValueEditorViewModel)value;
        }
        #endregion

        public StringValueEditorView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.Bind(ViewModel, vm => vm.Value, v => v.TextBox.Text).DisposeWith(d);
            });
        }
    }
}
