using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMacro.Model.Node
{
    public enum NodeType
    {
        EventNode, 
        Function, 
        FlowControl,
        Group,
        Literal, 
        Macro
    }
}
