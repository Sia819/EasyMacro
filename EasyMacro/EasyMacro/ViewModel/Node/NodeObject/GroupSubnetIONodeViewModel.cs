using System;
using EasyMacro.Model.Node;
using NodeNetwork.Toolkit.Group.AddEndpointDropPanel;
using NodeNetwork.ViewModels;
using EasyMacro.View.Node;
using ReactiveUI;
using System.Xml;
using System.Xml.Linq;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using System.Collections.Generic;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class GroupSubnetIONodeViewModel : CodeGenNodeViewModel, INodeSerializable, IExtendedXmlCustomSerializer
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

        public override NodeOutputViewModel GetOutputViewModel => null;

        public override void Serializer(XmlWriter xmlWriter, object obj)
        {
            NodeSerializer.SerializerOfNodeViewModel(ref xmlWriter, ref obj);
        }

        public override object Deserialize(XElement xElement)
        {
            GroupSubnetIONodeViewModel instance = (GroupSubnetIONodeViewModel)NodeSerializer.DeserializeOfNoveViewModel(ref xElement, this);
            return instance;
        }

        public override void Connect(INodeSerializable instance, List<INodeSerializable> obj)
        {
            throw new NotImplementedException();
        }

        public AddEndpointDropPanelViewModel AddEndpointDropPanelVM { get; set; }

        private readonly bool _isEntranceNode, _isExitNode;

        public GroupSubnetIONodeViewModel(NetworkViewModel subnet, bool isEntranceNode, bool isExitNode) : base(NodeType.Group)
        {
            this.Subnet = subnet;
            _isEntranceNode = isEntranceNode;
            _isExitNode = isExitNode;

            this.Hash = Common.HashGen.RandomHashGen(10);
        }

        public GroupSubnetIONodeViewModel(string hash) : this(new NetworkViewModel(), false, false)
        {
            this.Hash = hash;
        }
    }
}
