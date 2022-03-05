using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMacro.Model
{
    public interface IMacros
    {
        /// <summary>
        /// 매크로명령이 UI에 표시될 한국어 이름입니다.
        /// </summary>
        public string Text { get; }
        public bool IsSleep { get; }

    }
}
