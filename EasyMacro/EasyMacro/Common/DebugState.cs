namespace EasyMacro.Common
{
    public static class IsDebugMode
    {
        public static bool Check => System.Diagnostics.Debugger.IsAttached;
    }
}
