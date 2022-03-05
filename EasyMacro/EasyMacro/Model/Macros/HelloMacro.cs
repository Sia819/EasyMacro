using EasyMacroAPI;

namespace EasyMacro.Model
{
    public class HelloMacro : IMacros
    {
        private Hello action;
        public string Text => "인사 매크로";
        public bool IsSleep => false;

        public HelloMacro(IAction action)
        {
            this.action = (Hello)action;
        }

        public HelloMacro(string data)
        {
            this.action = new EasyMacroAPI.Hello(data);
        }


    }
}
