using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using EasyMacro.ViewModel.Node;
using EasyMacro.ViewModel.Node.NodeObject;
using ReactiveUI;

namespace EasyMacro.View.Node
{
    /// <summary>
    /// GroupNodeView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GroupNodeView : UserControl, IViewFor<GroupNodeViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(GroupNodeViewModel), typeof(GroupNodeView), new PropertyMetadata(null));

        public GroupNodeViewModel ViewModel
        {
            get => (GroupNodeViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (GroupNodeViewModel)value;
        }
        #endregion

        public GroupNodeView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                NodeView.ViewModel = this.ViewModel;
                Disposable.Create(() => NodeView.ViewModel = null).DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.NodeType, v => v.NodeView.Background, CodeGenNodeView.ConvertNodeTypeToBrush).DisposeWith(d);

            });
        }
    }
}
