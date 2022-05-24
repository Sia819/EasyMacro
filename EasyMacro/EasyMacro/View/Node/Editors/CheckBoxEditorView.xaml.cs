using System.Windows;
using System.Windows.Controls;
using EasyMacro.ViewModel.Node.Editors;
using ReactiveUI;

namespace EasyMacro.View.Node.Editors
{
    /// <summary>
    /// CheckBoxEditorView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CheckBoxEditorView : UserControl, IViewFor<CheckBoxEditorViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel),
                                        typeof(CheckBoxEditorViewModel),
                                        typeof(CheckBoxEditorView),
                                        new PropertyMetadata(null));

        public CheckBoxEditorViewModel ViewModel
        {
            get => (CheckBoxEditorViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (CheckBoxEditorViewModel)value;
        }
        #endregion

        public CheckBoxEditorView()
        {
            InitializeComponent();
            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => (bool)vm.Value, v => v.checkControl.IsChecked);
                this.Bind(ViewModel, vm => vm.CheckBoxContent, v => v.checkControl.Content);
            });
        }
    }
}
