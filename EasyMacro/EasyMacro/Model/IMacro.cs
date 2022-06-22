using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMacro.Model
{
    /// <summary>
    /// 이 인터페이스를 상속받은 객체는 매크로의 동작에 해당하는 클래스들로 간주되는것을 상정하여 만들어졌습니다.
    /// </summary>
    public interface IMacro
    {
        /// <summary>
        /// UI에 어떤 타입으로 표시될지 결정합니다.
        /// </summary>
        public MacroDisplayType DisplayType { get; }
        
        /// <summary>
        /// 매크로명령이 UI에 표시될 한국어 이름입니다.
        /// </summary>
        public string DisplayText { get; }
    }
}