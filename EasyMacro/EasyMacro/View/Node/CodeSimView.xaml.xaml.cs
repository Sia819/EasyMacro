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
using EasyMacro.ViewModel.Node;
using EasyMacro.ViewModel.Node.NodeObject;
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
        }
    }
}
