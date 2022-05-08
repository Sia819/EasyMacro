using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ReactiveUI;
using EasyMacro.ViewModel;
using System.Reactive.Disposables;

namespace EasyMacro.View
{
    /// <summary>
    /// DebugWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DebugWindow : Window, IViewFor<DebugWindowViewModel>
    {
        public DebugWindow()
        {
            InitializeComponent();
            /*
            this.WhenActivated(d =>
            {
                //this.OneWayBind(this.DataContext as DebugWindowViewModel,
                //                vm => vm.ListViewModel,
                //                v => v.nodeList.ViewModel
                //                ).DisposeWith(d);
                //this.OneWayBind(this.DataContext as MainWindowViewModel, vm => vm.NetworkViewModel, v => v.viewHost.ViewModel).DisposeWith(d);
                //this.OneWayBind(this.DataContext as MainWindowViewModel, vm => vm.ValueLabel, v => v.valueLabel.Content).DisposeWith(d);
            });
            */
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(DebugWindowViewModel), typeof(MainWindow), new PropertyMetadata(null));

        public DebugWindowViewModel ViewModel
        {
            get => (DebugWindowViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (DebugWindowViewModel)value;
        }
    }
}
