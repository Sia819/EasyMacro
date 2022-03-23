namespace EasyMacro.Common
{
    public static class DebugState
    {
        public static bool IsDebugStart => (System.Diagnostics.Debugger.IsAttached) ? true : false;
    }
}
