using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Reactive.Disposables;
using EasyMacro.ViewModel;
using EasyMacro.ViewModel.Node;
using ReactiveUI;

namespace EasyMacro.View.Node
{
    /// <summary>
    /// CodePreviewView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CodePreviewView : UserControl, IViewFor<CodePreviewViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(CodePreviewViewModel), typeof(CodePreviewView), new PropertyMetadata(null));

        public CodePreviewViewModel ViewModel
        {
            get => (CodePreviewViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (CodePreviewViewModel)value;
        }

        public CodePreviewView()
        {
            InitializeComponent();

            this.WhenActivated(d => {
                this.OneWayBind(ViewModel, vm => vm.CompiledCode, v => v.codeTextBlock.Text).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.CompilerError, v => v.errorTextBlock.Text).DisposeWith(d);
            });
        }
    }
}
