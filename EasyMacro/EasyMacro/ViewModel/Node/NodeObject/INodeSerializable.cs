using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public interface INodeSerializable
    {
        public string Hash { get; }
        public void Connect(INodeSerializable instance, List<INodeSerializable> obj);
    }
}
