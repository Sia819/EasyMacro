using System;
using EasyMacro.Model.Node;
using NodeNetwork.ViewModels;
using NodeNetwork.Toolkit.Group.AddEndpointDropPanel;
using EasyMacro.View.Node;
using ReactiveUI;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class GroupNodeViewModel : CodeGenNodeViewModel, INodeSerializable, IExtendedXmlCustomSerializer
    {
        static GroupNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new GroupNodeView(), typeof(IViewFor<GroupNodeViewModel>));
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
                AddEndpointDropPanelVM = new AddEndpointDropPanelViewModel
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
            GroupNodeViewModel instance = (GroupNodeViewModel)NodeSerializer.DeserializeOfNoveViewModel(ref xElement, new GroupNodeViewModel(Subnet));
            return instance;
        }

        public override void Connect(INodeSerializable instance, List<INodeSerializable> obj)
        {
            throw new NotImplementedException();
        }

        public AddEndpointDropPanelViewModel AddEndpointDropPanelVM { get; private set; }

        public GroupNodeViewModel(NetworkViewModel subnet) : base(NodeType.Group)
        {
            this.Name = "Group";
            this.Subnet = subnet;

            this.Hash = Common.HashGen.RandomHashGen(10);
        }

        public GroupNodeViewModel(string hash) : base(NodeType.Group)
        {
            this.Hash = hash;
        }
    }
}
