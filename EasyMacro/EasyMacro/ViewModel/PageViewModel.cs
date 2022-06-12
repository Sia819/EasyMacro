using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using DynamicData;
using EasyMacro.Model.Node;
using EasyMacro.ViewModel.Node;
using EasyMacro.ViewModel.Node.NodeObject;
using NodeNetwork.Toolkit.BreadcrumbBar;
using NodeNetwork.Toolkit.Group;
using NodeNetwork.Toolkit.Layout.ForceDirected;
using NodeNetwork.Toolkit.NodeList;
using NodeNetwork.ViewModels;
using ReactiveUI;

namespace EasyMacro.ViewModel
{
    class NetworkBreadcrumb : BreadcrumbViewModel
    {
        #region Network
        private NetworkViewModel _network;
        public NetworkViewModel Network
        {
            get => _network;
            set => this.RaiseAndSetIfChanged(ref _network, value);
        }
        #endregion
    }

    public class PageViewModel : ReactiveObject
    {
        private static PageViewModel _instance;
        public static PageViewModel Instance => _instance ??= new PageViewModel();

        #region Network
        private readonly ObservableAsPropertyHelper<NetworkViewModel> _network;
        public NetworkViewModel Network => _network.Value;
        #endregion

        public BreadcrumbBarViewModel NetworkBreadcrumbBar { get; } = new BreadcrumbBarViewModel();
        public NodeListViewModel NodeList { get; } = new NodeListViewModel();
        public CodePreviewViewModel CodePreview { get; } = new CodePreviewViewModel();
        public CodeSimViewModel CodeSim { get; } = CodeSimViewModel.Instance;

        public ReactiveCommand<Unit, Unit> AutoLayout { get; }
        public ReactiveCommand<Unit, Unit> StartAutoLayoutLive { get; }
        public ReactiveCommand<Unit, Unit> StopAutoLayoutLive { get; }

        public ReactiveCommand<Unit, Unit> GroupNodes { get; }
        public ReactiveCommand<Unit, Unit> UngroupNodes { get; }
        public ReactiveCommand<Unit, Unit> OpenGroup { get; }

        public StartNodeViewModel eventNode;

        private PageViewModel()
        {
            this.WhenAnyValue(vm => vm.NetworkBreadcrumbBar.ActiveItem).Cast<NetworkBreadcrumb>()
                .Select(b => b?.Network)
                .ToProperty(this, vm => vm.Network, out _network);
            NetworkBreadcrumbBar.ActivePath.Add(new NetworkBreadcrumb
            {
                Name = "Main",
                Network = new NetworkViewModel()
            });

            eventNode = new StartNodeViewModel { CanBeRemovedByUser = false };
            Network.Nodes.Add(eventNode);

            //NodeList.AddNodeType(() => new StartNodeViewModel());
            //NodeList.AddNodeType(() => new IntLiteralNode());
            //NodeList.AddNodeType(() => new PrintNode());
            //NodeList.AddNodeType(() => new TextLiteralNode());

            NodeList.AddNodeType(() => new ReStartNodeViewModel());
            NodeList.AddNodeType(() => new ForLoopNode());
            NodeList.AddNodeType(() => new DelayNodeViewModel());
            NodeList.AddNodeType(() => new CombInputKeyboardViewModel());
            NodeList.AddNodeType(() => new InputKeyboardNodeViewModel());
            NodeList.AddNodeType(() => new InputStringNodeViewModel());
            NodeList.AddNodeType(() => new InputMouseNodeViewModel());
            NodeList.AddNodeType(() => new RelativeMouseMoveNodeViewModel());
            NodeList.AddNodeType(() => new MouseClickNodeViewModel());
            NodeList.AddNodeType(() => new MouseMoveNodeViewModel());
            NodeList.AddNodeType(() => new TempletMatchNodeViewModel());

            var codeObservable = eventNode.OnClickFlow.Values.Connect().Select(_ => new StatementSequence(eventNode.OnClickFlow.Values.Items));
            codeObservable.BindTo(this, vm => vm.CodePreview.Code);
            codeObservable.BindTo(this, vm => vm.CodeSim.Code);

            ForceDirectedLayouter layouter = new ForceDirectedLayouter();
            AutoLayout = ReactiveCommand.Create(() => layouter.Layout(new Configuration { Network = Network }, 10000));
            StartAutoLayoutLive = ReactiveCommand.CreateFromObservable(() =>
                Observable.StartAsync(ct => layouter.LayoutAsync(new Configuration { Network = Network }, ct)).TakeUntil(StopAutoLayoutLive)
            );
            StopAutoLayoutLive = ReactiveCommand.Create(() => { }, StartAutoLayoutLive.IsExecuting);

            var grouper = new NodeGrouper
            {
                GroupNodeFactory = subnet => new GroupNodeViewModel(subnet),
                EntranceNodeFactory = () => new GroupSubnetIONodeViewModel(Network, true, false) { Name = "Group Input" },
                ExitNodeFactory = () => new GroupSubnetIONodeViewModel(Network, false, true) { Name = "Group Output" },
                SubNetworkFactory = () => new NetworkViewModel(),
                IOBindingFactory = (groupNode, entranceNode, exitNode) =>
                    new CodeNodeGroupIOBinding(groupNode, entranceNode, exitNode)
            };
            GroupNodes = ReactiveCommand.Create(() =>
            {
                var groupBinding = (CodeNodeGroupIOBinding)grouper.MergeIntoGroup(Network, Network.SelectedNodes.Items);
                ((GroupNodeViewModel)groupBinding.GroupNode).IOBinding = groupBinding;
                ((GroupSubnetIONodeViewModel)groupBinding.EntranceNode).IOBinding = groupBinding;
                ((GroupSubnetIONodeViewModel)groupBinding.ExitNode).IOBinding = groupBinding;
            }, this.WhenAnyObservable(vm => vm.Network.SelectedNodes.CountChanged).Select(c => c > 1));

            var isGroupNodeSelected = this.WhenAnyValue(vm => vm.Network)
                .Select(net => net.SelectedNodes.Connect())
                .Switch()
                .Select(_ => Network.SelectedNodes.Count == 1 && Network.SelectedNodes.Items.First() is GroupNodeViewModel);

            UngroupNodes = ReactiveCommand.Create(() =>
            {
                var selectedGroupNode = (GroupNodeViewModel)Network.SelectedNodes.Items.First();
                grouper.Ungroup(selectedGroupNode.IOBinding);
            }, isGroupNodeSelected);

            OpenGroup = ReactiveCommand.Create(() =>
            {
                var selectedGroupNode = (GroupNodeViewModel)Network.SelectedNodes.Items.First();
                NetworkBreadcrumbBar.ActivePath.Add(new NetworkBreadcrumb
                {
                    Network = selectedGroupNode.Subnet,
                    Name = selectedGroupNode.Name
                });
            }, isGroupNodeSelected);

            // 핫키 등록
            HookLib.GlobalKeyboardHook.StartKeyboardHook();
            HookLib.GlobalKeyboardHook.AddKeyboardHotkey(EasyMacroAPI.Model.Keys.F9, EasyMacroAPI.Model.KeyModifiers.None, StartMacro);
            //HookLib.GlobalKeyboardHook.AddKeyboardHotkey(EasyMacroAPI.Model.Keys.F9, EasyMacroAPI.Model.KeyModifiers.Control | EasyMacroAPI.Model.KeyModifiers.Alt, StartMacro);
            HookLib.GlobalKeyboardHook.AddKeyboardHotkey(EasyMacroAPI.Model.Keys.F10, EasyMacroAPI.Model.KeyModifiers.None, StopMacro);
        }

        public void Save()
        {
            IEnumerable<ReactiveObject> nodes = PageViewModel.Instance.Network.Nodes.Items;
            
            //IEnumerable<NodeViewModel> nodes = PageViewModel.Instance.Network.Nodes.Items;

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("MyFile.bin", FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, nodes);
            stream.Close();
        }

        public void Load()
        {

        }

        public void StartMacro(EasyMacroAPI.Model.Keys keys, EasyMacroAPI.Model.KeyModifiers keyModifiers)
        {
            //MessageBox.Show("F9");
            CodeSimViewModel.Instance.RunScript_ExcuteCommand();
        }

        public void StopMacro(EasyMacroAPI.Model.Keys keys, EasyMacroAPI.Model.KeyModifiers keyModifiers)
        {
            //MessageBox.Show("F10");
            CodeSimViewModel.Instance.TerminateThread();
        }

    }

}
