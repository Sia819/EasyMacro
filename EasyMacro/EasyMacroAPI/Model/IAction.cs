using EasyMacroAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMacroAPI
{
    public interface IAction
    {
        public MacroTypes MacroType { get; }

        public void Do();
    }
}
