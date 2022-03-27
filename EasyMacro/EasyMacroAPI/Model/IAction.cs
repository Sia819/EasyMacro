namespace EasyMacroAPI.Model
{
    public interface IAction
    {
        public MacroTypes MacroType { get; }

        public void Do();
    }
}
