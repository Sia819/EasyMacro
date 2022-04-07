namespace EasyMacro.Common
{
    public static class DebugState
    {
        public static bool IsDebugMode => System.Diagnostics.Debugger.IsAttached;
    }
}
