using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using EasyMacro.ViewModel.Node.Editors;
using ReactiveUI;

namespace EasyMacro.View.Node.Editors
{
    /// <summary>
    /// RunButtonView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RunButtonView : UserControl, IViewFor<RunButtonViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel),
                                        typeof(RunButtonViewModel),
                                        typeof(RunButtonView),
                                        new PropertyMetadata(null));

        public RunButtonViewModel ViewModel
        {
            get => (RunButtonViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (RunButtonViewModel)value;
        }

        public RunButtonView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.RunScript, v => v.runButton.Command)
                    .DisposeWith(d);
            });
        }
    }
}
