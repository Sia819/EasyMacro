using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using EasyMacro.ViewModel.Node;
using NodeNetwork.Views;
using ReactiveUI;

namespace EasyMacro.View.Node
{
    /// <summary>
    /// CodeGenConnectionView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CodeGenConnectionView : UserControl, IViewFor<CodeGenConnectionViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(CodeGenConnectionViewModel), typeof(CodeGenConnectionView), new PropertyMetadata(null));

        public CodeGenConnectionViewModel ViewModel
        {
            get => (CodeGenConnectionViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (CodeGenConnectionViewModel)value;
        }
        #endregion

        public CodeGenConnectionView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                ConnectionView.ViewModel = this.ViewModel;
                d(Disposable.Create(() => ConnectionView.ViewModel = null));
            });
        }
    }
}
