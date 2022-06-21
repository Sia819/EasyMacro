using System;
using EasyMacro.Model.Node;
using NodeNetwork.Toolkit.Group.AddEndpointDropPanel;
using NodeNetwork.ViewModels;
using EasyMacro.View.Node;
using ReactiveUI;
using System.Xml;
using System.Xml.Linq;
using ExtendedXmlSerializer.ExtensionModel.Xml;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class GroupSubnetIONodeViewModel : CodeGenNodeViewModel, IExtendedXmlCustomSerializer
    {
        static GroupSubnetIONodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new GroupSubnetIONodeView(), typeof(IViewFor<GroupSubnetIONodeViewModel>));
        }

        public NetworkViewModel Subnet { get; }

        #region IOBinding
        public CodeNodeGroupIOBinding IOBinding
        {
            get => _ioBinding;
            set
            {
                if (_ioBinding != null)
                {
                    throw new InvalidOperationException("IOBinding is already set.");
                }
                _ioBinding = value;
                AddEndpointDropPanelVM = new AddEndpointDropPanelViewModel(_isEntranceNode, _isExitNode)
                {
                    NodeGroupIOBinding = IOBinding
                };
            }
        }
        private CodeNodeGroupIOBinding _ioBinding;
        #endregion

        public override void Serializer(XmlWriter xmlWriter, object obj)
        {
            NodeSerializer.SerializerOfNodeViewModel(ref xmlWriter, ref obj);
        }

        public override object Deserialize(XElement xElement)
        {
            GroupSubnetIONodeViewModel instance = (GroupSubnetIONodeViewModel)NodeSerializer.DeserializeOfNoveViewModel(ref xElement, new GroupSubnetIONodeViewModel(Subnet, _isEntranceNode, _isExitNode));
            return instance;
        }

        public AddEndpointDropPanelViewModel AddEndpointDropPanelVM { get; set; }

        private readonly bool _isEntranceNode, _isExitNode;

        public GroupSubnetIONodeViewModel(NetworkViewModel subnet, bool isEntranceNode, bool isExitNode) : base(NodeType.Group)
        {
            this.Subnet = subnet;
            _isEntranceNode = isEntranceNode;
            _isExitNode = isExitNode;
        }
    }
}
