using System;

namespace EasyMacroAPI.Model
{
    [Flags]
    public enum KeyModifiers
    {
        NotSupport = -1,
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        Windows = 8,
        CapsLock = 16,
    }
}
