using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodeNetwork.Toolkit.NodeList;
using NodeNetwork.ViewModels;

namespace EasyMacro.ViewModel
{
    public class DebugWindowViewModel
    {
        public NodeListViewModel ListViewModel { get; } = new NodeListViewModel();
        public NetworkViewModel NetworkViewModel { get; } = new NetworkViewModel();

        public DebugWindowViewModel()
        {
            
        }

    }
}
