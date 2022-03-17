using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace EasyMacro.Model
{
    public class PropertiesModel
    {
        public string DisplayName { get; set; }
        public MacroCommand MacroType { get; set; }
    }
}
