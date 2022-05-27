namespace EasyMacroAPI.Model
{
    /// <summary>
    /// 매크로의 타입을 나타냅니다.<para/>
    /// 주로 IAction의 패러미터로 사용됩니다.
    /// </summary>
    public enum MacroTypes
    {
        Delay = 0,
        MouseMove = 1,
        MouseClick = 2,
        InputKeyboard = 3,
        CombInputKeyboard = 4,
        InputMouse = 5,
        InputString = 6,
        TempletMatch = 7,
        ScreenCapture = 8,
        WindowCapture = 9,
        FindWindowPosition = 10,
        FindMousePosition = 11,

        Folder = 99,
    }
}
