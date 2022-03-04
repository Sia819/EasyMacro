using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMacro.Model
{
    public class MacrosCommand
    {
        /// <summary>
        /// 매크로명령이 UI에 표시될 한국어 이름입니다.
        /// </summary>
        public string Name { get; }
        public bool IsSleep { get; }

        public MacrosCommand(string name)
        {
            this.Name = name;
        }

        public MacrosCommand(string name, bool IsSleep) : this(name)
        {
            this.IsSleep = IsSleep;
        }

    }
}
