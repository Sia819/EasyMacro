using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public interface INodeSerializable
    {
        public void Connect(INodeSerializable instance, List<INodeSerializable> obj);
    }
}
