using System;
using System.Windows;
using System.Windows.Controls;
using System.Reactive.Disposables;
using System.Windows.Media;
using EasyMacro.ViewModel.Node;
using NodeNetwork.Views;
using ReactiveUI;
using EasyMacro.Model.Node;

namespace EasyMacro.View.Node
{
    /// <summary>
    /// CodeGenNodeView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CodeGenNodeView : UserControl, IViewFor<CodeGenNodeViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(CodeGenNodeViewModel), typeof(CodeGenNodeView), new PropertyMetadata(null));

        public CodeGenNodeViewModel ViewModel
        {
            get => (CodeGenNodeViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (CodeGenNodeViewModel)value;
        }
        #endregion

        public CodeGenNodeView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                NodeView.ViewModel = this.ViewModel;
                Disposable.Create(() => NodeView.ViewModel = null).DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.NodeType, v => v.NodeView.Background, ConvertNodeTypeToBrush).DisposeWith(d);
            });
        }

        public static Brush ConvertNodeTypeToBrush(NodeType type)
        {
            switch (type)
            {
                case NodeType.EventNode: return new SolidColorBrush(Color.FromRgb(0x9b, 0x00, 0x00));
                case NodeType.FlowControl: return new SolidColorBrush(Color.FromRgb(0x49, 0x49, 0x49));
                case NodeType.Function: return new SolidColorBrush(Color.FromRgb(0x00, 0x39, 0xcb));
                case NodeType.Literal: return new SolidColorBrush(Color.FromRgb(0x00, 0x60, 0x0f));
                case NodeType.Group: return new SolidColorBrush(Color.FromRgb(0x7B, 0x1F, 0xA2));
                default: throw new Exception("Unsupported node type");
            }
        }
    }
}
