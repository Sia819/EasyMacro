using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
using ExtendedXmlSerializer;
using ExtendedXmlSerializer.ExtensionModel;
using ExtendedXmlSerializer.Configuration;
using System.Text;
using System.Xml;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using System.Windows;

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
        private ObservableAsPropertyHelper<NetworkViewModel> _network;
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

            
            // ListPanel에 추가하여 보여질 노드들
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
            // TODO : 지원하지 않는 노드
            // NodeList.AddNodeType(() => new StartNodeViewModel());
            // NodeList.AddNodeType(() => new IntLiteralNode());
            // NodeList.AddNodeType(() => new PrintNode());
            // NodeList.AddNodeType(() => new TextLiteralNode());

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
            HookLib.GlobalKeyboardHook.AddKeyboardHotkey(EasyMacroAPI.Model.Keys.F10, EasyMacroAPI.Model.KeyModifiers.None, StopMacro);
            //HookLib.GlobalKeyboardHook.AddKeyboardHotkey(EasyMacroAPI.Model.Keys.F9, EasyMacroAPI.Model.KeyModifiers.Control | EasyMacroAPI.Model.KeyModifiers.Alt, StartMacro);

        }

        private PageViewModel(object obj) : this()
        {
            // 강제적으로 부른 생성자



        }

        public void Save()
        {
            IEnumerable<ReactiveObject> nodes = PageViewModel.Instance.Network.Nodes.Items;
            List<IExtendedXmlCustomSerializer> nodesSerialize = new List<IExtendedXmlCustomSerializer>();
            
            IExtendedXmlSerializer serializer;
            serializer = new ConfigurationContainer().Type<StartNodeViewModel>()
                                                     .CustomSerializer<StartNodeViewModel>(typeof(StartNodeViewModel))
                                                     .Type<DelayNodeViewModel>()
                                                     .CustomSerializer<DelayNodeViewModel>(typeof(DelayNodeViewModel))
                                                     .Type<CombInputKeyboardViewModel>()
                                                     .CustomSerializer<CombInputKeyboardViewModel>(typeof(CombInputKeyboardViewModel))
                                                     .Type<ForLoopNode>()
                                                     .CustomSerializer<ForLoopNode>(typeof(ForLoopNode))
                                                     .Type<GroupNodeViewModel>()
                                                     .CustomSerializer<GroupNodeViewModel>(typeof(GroupNodeViewModel))
                                                     .Type<GroupSubnetIONodeViewModel>()
                                                     .CustomSerializer<GroupSubnetIONodeViewModel>(typeof(GroupSubnetIONodeViewModel))
                                                     .Type<InputKeyboardNodeViewModel>()
                                                     .CustomSerializer<InputKeyboardNodeViewModel>(typeof(InputKeyboardNodeViewModel))
                                                     .Type<InputMouseNodeViewModel>()
                                                     .CustomSerializer<InputMouseNodeViewModel>(typeof(InputMouseNodeViewModel))
                                                     .Type<InputStringNodeViewModel>()
                                                     .CustomSerializer<InputStringNodeViewModel>(typeof(InputStringNodeViewModel))
                                                     .Type<MouseClickNodeViewModel>()
                                                     .CustomSerializer<MouseClickNodeViewModel>(typeof(MouseClickNodeViewModel))
                                                     .Type<MouseMoveNodeViewModel>()
                                                     .CustomSerializer<MouseMoveNodeViewModel>(typeof(MouseMoveNodeViewModel))
                                                     .Type<OutputNodeViewModel>()
                                                     .CustomSerializer<OutputNodeViewModel>(typeof(OutputNodeViewModel))
                                                     .Type<RelativeMouseMoveNodeViewModel>()
                                                     .CustomSerializer<RelativeMouseMoveNodeViewModel>(typeof(RelativeMouseMoveNodeViewModel))
                                                     .Type<ReStartNodeViewModel>()
                                                     .CustomSerializer<ReStartNodeViewModel>(typeof(ReStartNodeViewModel))
                                                     .Type<TempletMatchNodeViewModel>()
                                                     .CustomSerializer<TempletMatchNodeViewModel>(typeof(TempletMatchNodeViewModel))
                                                     .Create();

            foreach (var node in nodes)
            {
                IExtendedXmlCustomSerializer serializeObject = node as IExtendedXmlCustomSerializer;
                if (serializeObject is null) { throw new Exception("해당 노드가 NodeSerialize를 구현하지 않았습니다!!"); }
                nodesSerialize.Add(serializeObject);
            }

            string xmlData = serializer.Serialize(nodesSerialize);// TODO : 모든 노드는 IExtendedXmlCustomSerializer를 구현해야하며, 모든 속성을 저장해야함.

            using (XmlTextWriter wr = new XmlTextWriter($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\{"TestSave.xml"}", Encoding.UTF8))
            {
                wr.Formatting = Formatting.Indented;
                XmlDocument document = new XmlDocument();
                document.LoadXml(xmlData);
                document.WriteContentTo(wr);
                wr.Flush();
            }
        }

        public void Load()
        {
            object obj = null;



            PageViewModel instance = new PageViewModel(obj);
            _instance.eventNode.Position = new Point(100, 50);
            
            // TODO : 여기에 저장된 파일을 로드하는 코드 작성

            // Change-Refresh View
            _instance = ((Application.Current.MainWindow as EasyMacro.View.MainWindow).mainFrame.Content as View.NodeEditPage).ViewModel = instance;


        }

        public void StartMacro(EasyMacroAPI.Model.Keys keys, EasyMacroAPI.Model.KeyModifiers keyModifiers)
        {
            CodeSimViewModel.Instance.RunScript_ExcuteCommand();
        }

        public void StopMacro(EasyMacroAPI.Model.Keys keys, EasyMacroAPI.Model.KeyModifiers keyModifiers)
        {
            CodeSimViewModel.Instance.TerminateThread();
        }

    }

}
