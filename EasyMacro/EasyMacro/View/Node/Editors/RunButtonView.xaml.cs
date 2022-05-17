using EasyMacro.ViewModel.Node.Editors;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                this.OneWayBind(ViewModel, vm => vm.RunScript, v => v.runButton.Command).DisposeWith(d);
            });
        }
    }
}
