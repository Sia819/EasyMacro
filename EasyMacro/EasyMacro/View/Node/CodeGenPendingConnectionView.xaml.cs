using System.Windows;
using System.Windows.Controls;
using System.Reactive.Disposables;
using EasyMacro.ViewModel.Node;
using NodeNetwork.Views;
using ReactiveUI;

namespace EasyMacro.View.Node
{
    /// <summary>
    /// CodeGenPendingConnectionView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CodeGenPendingConnectionView : UserControl, IViewFor<CodeGenPendingConnectionViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(CodeGenPendingConnectionViewModel), typeof(CodeGenPendingConnectionView), new PropertyMetadata(null));

        public CodeGenPendingConnectionViewModel ViewModel
        {
            get => (CodeGenPendingConnectionViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (CodeGenPendingConnectionViewModel)value;
        }
        #endregion

        public CodeGenPendingConnectionView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                PendingConnectionView.ViewModel = this.ViewModel;
                d(Disposable.Create(() => PendingConnectionView.ViewModel = null));
            });

        }
    }
}
