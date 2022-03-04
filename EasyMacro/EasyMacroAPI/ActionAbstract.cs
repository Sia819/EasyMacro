using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMacroAPI
{
    [Serializable]
    public abstract class ActionAbstract
    {
        public int index;

        public abstract void Do();
    }
}
