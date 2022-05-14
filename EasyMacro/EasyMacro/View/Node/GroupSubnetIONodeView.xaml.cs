using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using EasyMacro.ViewModel.Node.NodeObject;
using ReactiveUI;

namespace EasyMacro.View.Node
{
    /// <summary>
    /// GroupSubnetIONodeView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GroupSubnetIONodeView : UserControl, IViewFor<GroupSubnetIONodeViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(GroupSubnetIONodeViewModel), typeof(GroupSubnetIONodeView), new PropertyMetadata(null));

        public GroupSubnetIONodeViewModel ViewModel
        {
            get => (GroupSubnetIONodeViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (GroupSubnetIONodeViewModel)value;
        }
        #endregion

        public GroupSubnetIONodeView()
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
