using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using EasyMacro.ViewModel;
using ReactiveUI;

namespace EasyMacro.View
{
    /// <summary>
    /// NodeEditPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NodeEditPage : Page, IViewFor<PageViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(PageViewModel), typeof(NodeEditPage), new PropertyMetadata(null));
        
        public PageViewModel ViewModel
        {
            get => (PageViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (PageViewModel)value;
        }

        private readonly MenuItem groupNodesButton;
        private readonly MenuItem ungroupNodesButton;
        private readonly MenuItem openGroupButton;

        public NodeEditPage()
        {
            InitializeComponent();

            var nodeMenu = ((ContextMenu)Resources["nodeMenu"]).Items.OfType<MenuItem>();
            groupNodesButton = nodeMenu.First(c => c.Name == nameof(groupNodesButton));
            ungroupNodesButton = nodeMenu.First(c => c.Name == nameof(ungroupNodesButton));
            openGroupButton = nodeMenu.First(c => c.Name == nameof(openGroupButton));

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.Network, v => v.network.ViewModel).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.NodeList, v => v.nodeList.ViewModel).DisposeWith(d);
                //this.OneWayBind(ViewModel, vm => vm.CodePreview, v => v.codePreviewView.ViewModel).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.CodeSim, v => v.codeSimView.ViewModel).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.NetworkBreadcrumbBar, v => v.breadcrumbBar.ViewModel).DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.AutoLayout, v => v.autoLayoutButton);

                this.BindCommand(ViewModel, vm => vm.GroupNodes, v => v.groupNodesButton).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.UngroupNodes, v => v.ungroupNodesButton).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.OpenGroup, v => v.openGroupButton).DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.StartAutoLayoutLive, v => v.startAutoLayoutLiveButton);
                this.WhenAnyObservable(v => v.ViewModel.StartAutoLayoutLive.IsExecuting)
                    .Select((isRunning) => isRunning ? Visibility.Collapsed : Visibility.Visible)
                    .BindTo(this, v => v.startAutoLayoutLiveButton.Visibility);

                this.BindCommand(ViewModel, vm => vm.StopAutoLayoutLive, v => v.stopAutoLayoutLiveButton);
                this.WhenAnyObservable(v => v.ViewModel.StartAutoLayoutLive.IsExecuting)
                    .Select((isRunning) => isRunning ? Visibility.Visible : Visibility.Collapsed)
                    .BindTo(this, v => v.stopAutoLayoutLiveButton.Visibility);
            });

            this.ViewModel = PageViewModel.Instance;
        }

        private void debug_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
