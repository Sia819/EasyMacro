﻿using System;
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
using System.Windows.Forms;

using System.Windows;
using NodeNetwork;
using NodeNetwork.Toolkit;

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

        public BreadcrumbBarViewModel NetworkBreadcrumbBar { get; }
        public NodeListViewModel NodeList { get; }
        public CodeSimViewModel CodeSim { get; }
        public ReactiveCommand<Unit, Unit> AutoLayout { get; }
        public ReactiveCommand<Unit, Unit> StartAutoLayoutLive { get; }
        public ReactiveCommand<Unit, Unit> StopAutoLayoutLive { get; }
        public ReactiveCommand<Unit, Unit> GroupNodes { get; }
        public ReactiveCommand<Unit, Unit> UngroupNodes { get; }
        public ReactiveCommand<Unit, Unit> OpenGroup { get; }
        public string ValueLabel { get; set; }

        public StartNodeViewModel startNode;

        public static bool IsLoading = false;
        

        private PageViewModel()
        {
            //Initialize Properties
            this.NetworkBreadcrumbBar = new BreadcrumbBarViewModel();
            this.CodeSim = CodeSimViewModel.Instance;

            this.WhenAnyValue(vm => vm.NetworkBreadcrumbBar.ActiveItem).Cast<NetworkBreadcrumb>()
                .Select(b => b?.Network)
                .ToProperty(this, vm => vm.Network, out _network);

            NetworkBreadcrumbBar.ActivePath.Add(new NetworkBreadcrumb
            {
                Name = "Main",
                Network = new NetworkViewModel()
            });

            #region Auto Layout Settings
            ForceDirectedLayouter layouter = new ForceDirectedLayouter();
            AutoLayout = ReactiveCommand.Create(() => layouter.Layout(new Configuration { Network = Network }, 10000));
            StartAutoLayoutLive = ReactiveCommand.CreateFromObservable(() =>
                Observable.StartAsync(ct => layouter.LayoutAsync(new Configuration { Network = Network }, ct)).TakeUntil(StopAutoLayoutLive)
            );
            StopAutoLayoutLive = ReactiveCommand.Create(() => { }, StartAutoLayoutLive.IsExecuting);
            #endregion

            // Initialize Group Properties
            NodeGrouper grouper = new NodeGrouper
            {
                GroupNodeFactory = (subnet) => new GroupNodeViewModel(subnet),
                EntranceNodeFactory = () => new GroupSubnetIONodeViewModel(Network, true, false) { Name = "Group Input" },
                ExitNodeFactory = () => new GroupSubnetIONodeViewModel(Network, false, true) { Name = "Group Output" },
                SubNetworkFactory = () => new NetworkViewModel(),
                IOBindingFactory = (groupNode, entranceNode, exitNode) => new CodeNodeGroupIOBinding(groupNode, entranceNode, exitNode)
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

            // ListPanel에 추가하여 보여질 노드들
            this.NodeList = new NodeListViewModel();
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

            if (IsLoading is false)
            {
                startNode = new StartNodeViewModel { CanBeRemovedByUser = false };
                Network.Nodes.Add(startNode);

                var codeObservable = startNode.FlowOut.Values.Connect().Select(_ => new StatementSequence(startNode.FlowOut.Values.Items));
                codeObservable.BindTo(this, vm => vm.CodeSim.Code);
            }

            // 핫키 등록
            HookLib.GlobalKeyboardHook.StartKeyboardHook();
            HookLib.GlobalKeyboardHook.AddKeyboardHotkey(EasyMacroAPI.Model.Keys.F9, EasyMacroAPI.Model.KeyModifiers.None, StartMacro);
            HookLib.GlobalKeyboardHook.AddKeyboardHotkey(EasyMacroAPI.Model.Keys.F10, EasyMacroAPI.Model.KeyModifiers.None, StopMacro);
        }

        private PageViewModel(List<INodeSerializable> obj) : this()
        {
            // 강제적으로 부른 생성자
            foreach (var node in obj)
            {
                Network.Nodes.Add((NodeViewModel)node);
            }

            startNode = Network.Nodes.Items.First() as StartNodeViewModel;
            var codeObservable = startNode.FlowOut.Values.Connect().Select(_ => new StatementSequence(startNode.FlowOut.Values.Items));
            codeObservable.BindTo(this, vm => vm.CodeSim.Code);
        }

        public void Save()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "XML File|*.xml|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true,
                FileName = "Save.xml"
            };
            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            IExtendedXmlSerializer serializer;
            serializer = new ConfigurationContainer().WithUnknownContent()
                                                     .Continue()
                                                     .CustomSerializer<StartNodeViewModel>(typeof(StartNodeViewModel))
                                                     .CustomSerializer<DelayNodeViewModel>(typeof(DelayNodeViewModel))
                                                     .CustomSerializer<CombInputKeyboardViewModel>(typeof(CombInputKeyboardViewModel))
                                                     .CustomSerializer<ForLoopNode>(typeof(ForLoopNode))
                                                     // .CustomSerializer<GroupNodeViewModel>(typeof(GroupNodeViewModel))
                                                     // .CustomSerializer<GroupSubnetIONodeViewModel>(typeof(GroupSubnetIONodeViewModel))
                                                     .CustomSerializer<InputKeyboardNodeViewModel>(typeof(InputKeyboardNodeViewModel))
                                                     .CustomSerializer<InputMouseNodeViewModel>(typeof(InputMouseNodeViewModel))
                                                     .CustomSerializer<InputStringNodeViewModel>(typeof(InputStringNodeViewModel))
                                                     .CustomSerializer<MouseClickNodeViewModel>(typeof(MouseClickNodeViewModel))
                                                     .CustomSerializer<MouseMoveNodeViewModel>(typeof(MouseMoveNodeViewModel))
                                                     .CustomSerializer<OutputNodeViewModel>(typeof(OutputNodeViewModel))
                                                     .CustomSerializer<RelativeMouseMoveNodeViewModel>(typeof(RelativeMouseMoveNodeViewModel))
                                                     .CustomSerializer<ReStartNodeViewModel>(typeof(ReStartNodeViewModel))
                                                     .CustomSerializer<TempletMatchNodeViewModel>(typeof(TempletMatchNodeViewModel))
                                                     .Create();

            List<INodeSerializable> nodes = new List<INodeSerializable>();

            foreach (var node in Network.Nodes.Items)
            {
                INodeSerializable serializeObject = node as INodeSerializable;
                if (serializeObject is null) { throw new Exception("해당 노드가 NodeSerialize를 구현하지 않았습니다!!"); }
                nodes.Add(serializeObject);
            }

            var xmlData = serializer.Serialize(nodes);// TODO : 모든 노드는 IExtendedXmlCustomSerializer를 구현해야하며, 모든 속성을 저장해야함.

            string savepath = saveFileDialog.FileName.ToString();
            using (XmlTextWriter wr = new XmlTextWriter(savepath, Encoding.UTF8))
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
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "XML File|*.xml";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;
            openFileDialog.FileName = "Save.xml";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                IExtendedXmlSerializer serializer;
                serializer = new ConfigurationContainer().WithUnknownContent()
                                                         .Continue()
                                                         .CustomSerializer<StartNodeViewModel>(typeof(NodeSerializer))
                                                         .CustomSerializer<DelayNodeViewModel>(typeof(NodeSerializer))
                                                         .CustomSerializer<CombInputKeyboardViewModel>(typeof(NodeSerializer))
                                                         .CustomSerializer<ForLoopNode>(typeof(NodeSerializer))
                                                         // .CustomSerializer<GroupNodeViewModel>(typeof(NodeSerializer))
                                                         // .CustomSerializer<GroupSubnetIONodeViewModel>(typeof(NodeSerializer))
                                                         .CustomSerializer<InputKeyboardNodeViewModel>(typeof(NodeSerializer))
                                                         .CustomSerializer<InputMouseNodeViewModel>(typeof(NodeSerializer))
                                                         .CustomSerializer<InputStringNodeViewModel>(typeof(NodeSerializer))
                                                         .CustomSerializer<MouseClickNodeViewModel>(typeof(NodeSerializer))
                                                         .CustomSerializer<MouseMoveNodeViewModel>(typeof(NodeSerializer))
                                                         .CustomSerializer<OutputNodeViewModel>(typeof(NodeSerializer))
                                                         .CustomSerializer<RelativeMouseMoveNodeViewModel>(typeof(NodeSerializer))
                                                         .CustomSerializer<ReStartNodeViewModel>(typeof(NodeSerializer))
                                                         .CustomSerializer<TempletMatchNodeViewModel>(typeof(NodeSerializer))
                                                         .Create();

                List<INodeSerializable> obj = null;
                string savepath = openFileDialog.FileName;

                IsLoading = true;

                using (var reader = new StreamReader(savepath))
                {
                    obj = serializer.Deserialize<List<INodeSerializable>>(reader); //
                }

                PageViewModel instance = new PageViewModel(obj);

                foreach (INodeSerializable i in obj)
                {
                    i.Connect(i, obj);
                }

                // Change-Refresh ViewModel
                _instance = ((System.Windows.Application.Current.MainWindow as EasyMacro.View.MainWindow).mainFrame.Content as View.NodeEditPage).ViewModel = instance;
            }
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
