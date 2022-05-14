﻿using System;
using EasyMacro.Model.Node;
using NodeNetwork.ViewModels;
using NodeNetwork.Toolkit.Group.AddEndpointDropPanel;
using EasyMacro.View.Node;
using ReactiveUI;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class GroupNodeViewModel : CodeGenNodeViewModel
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

        public AddEndpointDropPanelViewModel AddEndpointDropPanelVM { get; private set; }

        public GroupNodeViewModel(NetworkViewModel subnet) : base(NodeType.Group)
        {
            this.Name = "Group";
            this.Subnet = subnet;
        }
    }
}
