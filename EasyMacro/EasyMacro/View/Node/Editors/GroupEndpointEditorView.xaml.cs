using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using EasyMacro.ViewModel.Node;
using EasyMacro.ViewModel.Node.Editors;
using ReactiveUI;

namespace EasyMacro.View.Node.Editors
{
    /// <summary>
    /// GroupEndpointEditorView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GroupEndpointEditorView : UserControl, IViewFor<IGroupEndpointEditorViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(IGroupEndpointEditorViewModel), typeof(GroupEndpointEditorView), new PropertyMetadata(null));

        public IGroupEndpointEditorViewModel ViewModel
        {
            get => (IGroupEndpointEditorViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (IGroupEndpointEditorViewModel)value;
        }
        #endregion

        public GroupEndpointEditorView()
        {
            InitializeComponent();

            this.WhenAnyValue(v => v.editNameButton.IsChecked)
                .Select(isChecked => (isChecked ?? false) ? Visibility.Visible : Visibility.Collapsed)
                .BindTo(this, v => v.nameTextBox.Visibility);

            this.WhenActivated(d =>
            {
                this.Bind(ViewModel, vm => vm.Endpoint.Name, v => v.nameTextBox.Text).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.MoveUp, v => v.upButton).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.MoveDown, v => v.downButton).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.Delete, v => v.deleteButton).DisposeWith(d);
            });
        }
    }
}
