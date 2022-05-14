using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using EasyMacro.ViewModel.Node;
using ReactiveUI;

namespace EasyMacro.View.Node
{
    /// <summary>
    /// CodeSimView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CodeSimView : UserControl, IViewFor<CodeSimViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(CodeSimViewModel), typeof(CodeSimView), new PropertyMetadata(null));

        public CodeSimViewModel ViewModel
        {
            get => (CodeSimViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (CodeSimViewModel)value;
        }

        public CodeSimView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.RunScript, v => v.runButton.Command).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.ClearOutput, v => v.clearButton.Command).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.Output, v => v.outputTextBlock.Text).DisposeWith(d);
            });
        }
    }
}
