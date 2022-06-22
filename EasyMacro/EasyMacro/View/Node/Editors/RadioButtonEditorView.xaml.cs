using System.Windows;
using System.Windows.Controls;
using System.Reactive.Disposables;
using EasyMacro.ViewModel.Node.Editors;
using ReactiveUI;

namespace EasyMacro.View.Node.Editors
{
    /// <summary>
    /// RadioButtonEditorView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RadioButtonEditorView : UserControl, IViewFor<RadioButtonEditorViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(RadioButtonEditorViewModel), typeof(RadioButtonEditorView), new PropertyMetadata(null));

        public RadioButtonEditorViewModel ViewModel
        {
            get => (RadioButtonEditorViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (RadioButtonEditorViewModel)value;
        }
        #endregion ViewModel

        public RadioButtonEditorView()
        {
            InitializeComponent();

           

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.MyList, v => v.radioList.ItemsSource).DisposeWith(d);
            });
        }

        
    }
}
