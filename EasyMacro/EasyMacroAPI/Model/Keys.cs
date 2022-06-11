using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace EasyMacroAPI.Model
{

    //
    // 요약:
    //     Specifies the possible key values on a keyboard.
    // [TypeConverter(typeof(KeyConverter))]
    // [ValueSerializer(typeof(KeyValueSerializer))]
    /*
    public enum Keys
    {
        
        //
        // 요약:
        //     No key pressed.
        None = 0,
        //
        // 요약:
        //     The Cancel key.
        Cancel = 1,
        //
        // 요약:
        //     The Backspace key.
        Back = 2,
        //
        // 요약:
        //     The Tab key.
        Tab = 3,
        //
        // 요약:
        //     The Linefeed key.
        LineFeed = 4,
        //
        // 요약:
        //     The Clear key.
        Clear = 5,
        //
        // 요약:
        //     The Enter key.
        Enter = 6,
        //
        // 요약:
        //     The Return key.
        Return = 6,
        //
        // 요약:
        //     The Pause key.
        Pause = 7,
        //
        // 요약:
        //     The Caps Lock key.
        Capital = 8,
        //
        // 요약:
        //     The Caps Lock key.
        CapsLock = 8,
        //
        // 요약:
        //     The IME Hangul mode key.
        HangulMode = 9,
        //
        // 요약:
        //     The IME Kana mode key.
        KanaMode = 9,
        //
        // 요약:
        //     The IME Junja mode key.
        JunjaMode = 10,
        //
        // 요약:
        //     The IME Final mode key.
        FinalMode = 11,
        //
        // 요약:
        //     The IME Hanja mode key.
        HanjaMode = 12,
        //
        // 요약:
        //     The IME Kanji mode key.
        KanjiMode = 12,
        //
        // 요약:
        //     The ESC key.
        Escape = 13,
        //
        // 요약:
        //     The IME Convert key.
        ImeConvert = 14,
        //
        // 요약:
        //     The IME NonConvert key.
        ImeNonConvert = 0xF,
        //
        // 요약:
        //     The IME Accept key.
        ImeAccept = 0x10,
        //
        // 요약:
        //     The IME Mode change request.
        ImeModeChange = 17,
        //
        // 요약:
        //     The Spacebar key.
        Space = 18,
        //
        // 요약:
        //     The Page Up key.
        PageUp = 19,
        //
        // 요약:
        //     The Page Up key.
        Prior = 19,
        //
        // 요약:
        //     The Page Down key.
        Next = 20,
        //
        // 요약:
        //     The Page Down key.
        PageDown = 20,
        //
        // 요약:
        //     The End key.
        End = 21,
        //
        // 요약:
        //     The Home key.
        Home = 22,
        //
        // 요약:
        //     The Left Arrow key.
        Left = 23,
        //
        // 요약:
        //     The Up Arrow key.
        Up = 24,
        //
        // 요약:
        //     The Right Arrow key.
        Right = 25,
        //
        // 요약:
        //     The Down Arrow key.
        Down = 26,
        //
        // 요약:
        //     The Select key.
        Select = 27,
        //
        // 요약:
        //     The Print key.
        Print = 28,
        //
        // 요약:
        //     The Execute key.
        Execute = 29,
        //
        // 요약:
        //     The Print Screen key.
        PrintScreen = 30,
        //
        // 요약:
        //     The Print Screen key.
        Snapshot = 30,
        //
        // 요약:
        //     The Insert key.
        Insert = 0x1F,
        //
        // 요약:
        //     The Delete key.
        Delete = 0x20,
        //
        // 요약:
        //     The Help key.
        Help = 33,
        //
        // 요약:
        //     The 0 (zero) key.
        D0 = 34,
        //
        // 요약:
        //     The 1 (one) key.
        D1 = 35,
        //
        // 요약:
        //     The 2 key.
        D2 = 36,
        //
        // 요약:
        //     The 3 key.
        D3 = 37,
        //
        // 요약:
        //     The 4 key.
        D4 = 38,
        //
        // 요약:
        //     The 5 key.
        D5 = 39,
        //
        // 요약:
        //     The 6 key.
        D6 = 40,
        //
        // 요약:
        //     The 7 key.
        D7 = 41,
        //
        // 요약:
        //     The 8 key.
        D8 = 42,
        //
        // 요약:
        //     The 9 key.
        D9 = 43,
        //
        // 요약:
        //     The A key.
        A = 44,
        //
        // 요약:
        //     The B key.
        B = 45,
        //
        // 요약:
        //     The C key.
        C = 46,
        //
        // 요약:
        //     The D key.
        D = 47,
        //
        // 요약:
        //     The E key.
        E = 48,
        //
        // 요약:
        //     The F key.
        F = 49,
        //
        // 요약:
        //     The G key.
        G = 50,
        //
        // 요약:
        //     The H key.
        H = 51,
        //
        // 요약:
        //     The I key.
        I = 52,
        //
        // 요약:
        //     The J key.
        J = 53,
        //
        // 요약:
        //     The K key.
        K = 54,
        //
        // 요약:
        //     The L key.
        L = 55,
        //
        // 요약:
        //     The M key.
        M = 56,
        //
        // 요약:
        //     The N key.
        N = 57,
        //
        // 요약:
        //     The O key.
        O = 58,
        //
        // 요약:
        //     The P key.
        P = 59,
        //
        // 요약:
        //     The Q key.
        Q = 60,
        //
        // 요약:
        //     The R key.
        R = 61,
        //
        // 요약:
        //     The S key.
        S = 62,
        //
        // 요약:
        //     The T key.
        T = 0x3F,
        //
        // 요약:
        //     The U key.
        U = 0x40,
        //
        // 요약:
        //     The V key.
        V = 65,
        //
        // 요약:
        //     The W key.
        W = 66,
        //
        // 요약:
        //     The X key.
        X = 67,
        //
        // 요약:
        //     The Y key.
        Y = 68,
        //
        // 요약:
        //     The Z key.
        Z = 69,
        //
        // 요약:
        //     The left Windows logo key (Microsoft Natural Keyboard).
        LWin = 70,
        //
        // 요약:
        //     The right Windows logo key (Microsoft Natural Keyboard).
        RWin = 71,
        //
        // 요약:
        //     The Application key (Microsoft Natural Keyboard). Also known as the Menu key,
        //     as it displays an application-specific context menu.
        Apps = 72,
        //
        // 요약:
        //     The Computer Sleep key.
        Sleep = 73,
        //
        // 요약:
        //     The 0 key on the numeric keypad.
        NumPad0 = 74,
        //
        // 요약:
        //     The 1 key on the numeric keypad.
        NumPad1 = 75,
        //
        // 요약:
        //     The 2 key on the numeric keypad.
        NumPad2 = 76,
        //
        // 요약:
        //     The 3 key on the numeric keypad.
        NumPad3 = 77,
        //
        // 요약:
        //     The 4 key on the numeric keypad.
        NumPad4 = 78,
        //
        // 요약:
        //     The 5 key on the numeric keypad.
        NumPad5 = 79,
        //
        // 요약:
        //     The 6 key on the numeric keypad.
        NumPad6 = 80,
        //
        // 요약:
        //     The 7 key on the numeric keypad.
        NumPad7 = 81,
        //
        // 요약:
        //     The 8 key on the numeric keypad.
        NumPad8 = 82,
        //
        // 요약:
        //     The 9 key on the numeric keypad.
        NumPad9 = 83,
        //
        // 요약:
        //     The Multiply key.
        Multiply = 84,
        //
        // 요약:
        //     The Add key.
        Add = 85,
        //
        // 요약:
        //     The Separator key.
        Separator = 86,
        //
        // 요약:
        //     The Subtract key.
        Subtract = 87,
        //
        // 요약:
        //     The Decimal key.
        Decimal = 88,
        //
        // 요약:
        //     The Divide key.
        Divide = 89,
        //
        // 요약:
        //     The F1 key.
        F1 = 90,
        //
        // 요약:
        //     The F2 key.
        F2 = 91,
        //
        // 요약:
        //     The F3 key.
        F3 = 92,
        //
        // 요약:
        //     The F4 key.
        F4 = 93,
        //
        // 요약:
        //     The F5 key.
        F5 = 94,
        //
        // 요약:
        //     The F6 key.
        F6 = 95,
        //
        // 요약:
        //     The F7 key.
        F7 = 96,
        //
        // 요약:
        //     The F8 key.
        F8 = 97,
        //
        // 요약:
        //     The F9 key.
        F9 = 98,
        //
        // 요약:
        //     The F10 key.
        F10 = 99,
        //
        // 요약:
        //     The F11 key.
        F11 = 100,
        //
        // 요약:
        //     The F12 key.
        F12 = 101,
        //
        // 요약:
        //     The F13 key.
        F13 = 102,
        //
        // 요약:
        //     The F14 key.
        F14 = 103,
        //
        // 요약:
        //     The F15 key.
        F15 = 104,
        //
        // 요약:
        //     The F16 key.
        F16 = 105,
        //
        // 요약:
        //     The F17 key.
        F17 = 106,
        //
        // 요약:
        //     The F18 key.
        F18 = 107,
        //
        // 요약:
        //     The F19 key.
        F19 = 108,
        //
        // 요약:
        //     The F20 key.
        F20 = 109,
        //
        // 요약:
        //     The F21 key.
        F21 = 110,
        //
        // 요약:
        //     The F22 key.
        F22 = 111,
        //
        // 요약:
        //     The F23 key.
        F23 = 112,
        //
        // 요약:
        //     The F24 key.
        F24 = 113,
        //
        // 요약:
        //     The Num Lock key.
        NumLock = 114,
        //
        // 요약:
        //     The Scroll Lock key.
        Scroll = 115,
        //
        // 요약:
        //     The left Shift key.
        LeftShift = 116,
        //
        // 요약:
        //     The right Shift key.
        RightShift = 117,
        //
        // 요약:
        //     The left CTRL key.
        LeftCtrl = 118,
        //
        // 요약:
        //     The right CTRL key.
        RightCtrl = 119,
        //
        // 요약:
        //     The left ALT key.
        LeftAlt = 120,
        //
        // 요약:
        //     The right ALT key.
        RightAlt = 121,
        //
        // 요약:
        //     The Browser Back key.
        BrowserBack = 122,
        //
        // 요약:
        //     The Browser Forward key.
        BrowserForward = 123,
        //
        // 요약:
        //     The Browser Refresh key.
        BrowserRefresh = 124,
        //
        // 요약:
        //     The Browser Stop key.
        BrowserStop = 125,
        //
        // 요약:
        //     The Browser Search key.
        BrowserSearch = 126,
        //
        // 요약:
        //     The Browser Favorites key.
        BrowserFavorites = 0x7F,
        //
        // 요약:
        //     The Browser Home key.
        BrowserHome = 0x80,
        //
        // 요약:
        //     The Volume Mute key.
        VolumeMute = 129,
        //
        // 요약:
        //     The Volume Down key.
        VolumeDown = 130,
        //
        // 요약:
        //     The Volume Up key.
        VolumeUp = 131,
        //
        // 요약:
        //     The Media Next Track key.
        MediaNextTrack = 132,
        //
        // 요약:
        //     The Media Previous Track key.
        MediaPreviousTrack = 133,
        //
        // 요약:
        //     The Media Stop key.
        MediaStop = 134,
        //
        // 요약:
        //     The Media Play Pause key.
        MediaPlayPause = 135,
        //
        // 요약:
        //     The Launch Mail key.
        LaunchMail = 136,
        //
        // 요약:
        //     The Select Media key.
        SelectMedia = 137,
        //
        // 요약:
        //     The Launch Application1 key.
        LaunchApplication1 = 138,
        //
        // 요약:
        //     The Launch Application2 key.
        LaunchApplication2 = 139,
        //
        // 요약:
        //     The OEM 1 key.
        Oem1 = 140,
        //
        // 요약:
        //     The OEM Semicolon key.
        OemSemicolon = 140,
        //
        // 요약:
        //     The OEM Addition key.
        OemPlus = 141,
        //
        // 요약:
        //     The OEM Comma key.
        OemComma = 142,
        //
        // 요약:
        //     The OEM Minus key.
        OemMinus = 143,
        //
        // 요약:
        //     The OEM Period key.
        OemPeriod = 144,
        //
        // 요약:
        //     The OEM 2 key.
        Oem2 = 145,
        //
        // 요약:
        //     The OEM Question key.
        OemQuestion = 145,
        //
        // 요약:
        //     The OEM 3 key.
        Oem3 = 146,
        //
        // 요약:
        //     The OEM Tilde key.
        OemTilde = 146,
        //
        // 요약:
        //     The ABNT_C1 (Brazilian) key.
        AbntC1 = 147,
        //
        // 요약:
        //     The ABNT_C2 (Brazilian) key.
        AbntC2 = 148,
        //
        // 요약:
        //     The OEM 4 key.
        Oem4 = 149,
        //
        // 요약:
        //     The OEM Open Brackets key.
        OemOpenBrackets = 149,
        //
        // 요약:
        //     The OEM 5 key.
        Oem5 = 150,
        //
        // 요약:
        //     The OEM Pipe key.
        OemPipe = 150,
        //
        // 요약:
        //     The OEM 6 key.
        Oem6 = 151,
        //
        // 요약:
        //     The OEM Close Brackets key.
        OemCloseBrackets = 151,
        //
        // 요약:
        //     The OEM 7 key.
        Oem7 = 152,
        //
        // 요약:
        //     The OEM Quotes key.
        OemQuotes = 152,
        //
        // 요약:
        //     The OEM 8 key.
        Oem8 = 153,
        //
        // 요약:
        //     The OEM 102 key.
        Oem102 = 154,
        //
        // 요약:
        //     The OEM Backslash key.
        OemBackslash = 154,
        //
        // 요약:
        //     A special key masking the real key being processed by an IME.
        ImeProcessed = 155,
        //
        // 요약:
        //     A special key masking the real key being processed as a system key.
        System = 156,
        //
        // 요약:
        //     The DBE_ALPHANUMERIC key.
        DbeAlphanumeric = 157,
        //
        // 요약:
        //     The OEM ATTN key.
        OemAttn = 157,
        //
        // 요약:
        //     The DBE_KATAKANA key.
        DbeKatakana = 158,
        //
        // 요약:
        //     The OEM FINISH key.
        OemFinish = 158,
        //
        // 요약:
        //     The DBE_HIRAGANA key.
        DbeHiragana = 159,
        //
        // 요약:
        //     The OEM COPY key.
        OemCopy = 159,
        //
        // 요약:
        //     The DBE_SBCSCHAR key.
        DbeSbcsChar = 160,
        //
        // 요약:
        //     The OEM AUTO key.
        OemAuto = 160,
        //
        // 요약:
        //     The DBE_DBCSCHAR key.
        DbeDbcsChar = 161,
        //
        // 요약:
        //     The OEM ENLW key.
        OemEnlw = 161,
        //
        // 요약:
        //     The DBE_ROMAN key.
        DbeRoman = 162,
        //
        // 요약:
        //     The OEM BACKTAB key.
        OemBackTab = 162,
        //
        // 요약:
        //     The ATTN key.
        Attn = 163,
        //
        // 요약:
        //     The DBE_NOROMAN key.
        DbeNoRoman = 163,
        //
        // 요약:
        //     The CRSEL key.
        CrSel = 164,
        //
        // 요약:
        //     The DBE_ENTERWORDREGISTERMODE key.
        DbeEnterWordRegisterMode = 164,
        //
        // 요약:
        //     The DBE_ENTERIMECONFIGMODE key.
        DbeEnterImeConfigureMode = 165,
        //
        // 요약:
        //     The EXSEL key.
        ExSel = 165,
        //
        // 요약:
        //     The DBE_FLUSHSTRING key.
        DbeFlushString = 166,
        //
        // 요약:
        //     The ERASE EOF key.
        EraseEof = 166,
        //
        // 요약:
        //     The DBE_CODEINPUT key.
        DbeCodeInput = 167,
        //
        // 요약:
        //     The PLAY key.
        Play = 167,
        //
        // 요약:
        //     The DBE_NOCODEINPUT key.
        DbeNoCodeInput = 168,
        //
        // 요약:
        //     The ZOOM key.
        Zoom = 168,
        //
        // 요약:
        //     The DBE_DETERMINESTRING key.
        DbeDetermineString = 169,
        //
        // 요약:
        //     A constant reserved for future use.
        NoName = 169,
        //
        // 요약:
        //     The DBE_ENTERDLGCONVERSIONMODE key.
        DbeEnterDialogConversionMode = 170,
        //
        // 요약:
        //     The PA1 key.
        Pa1 = 170,
        //
        // 요약:
        //     The OEM Clear key.
        OemClear = 171,
        //
        // 요약:
        //     The key is used with another key to create a single combined character.
        DeadCharProcessed = 172
    }
    */


    
    //
    // 요약:
    //     Specifies key codes and modifiers.
    //[Flags]
    public enum Keys
    {
        // 어떤 키 이던지 반응합니다. 오직 EasyMacro에서만 사용할 수 있습니다.
        AnyKey = -2,
        // 등록되지 않은 키 입니다. 오직 EasyMacro에서만 사용할 수 있습니다.
        NotSupport = -1,

        //
        // 요약:
        //     The bitmask to extract modifiers from a key value.
        Modifiers = -65536,
        //
        // 요약:
        //     No key pressed.
        None = 0,
        //
        // 요약:
        //     The left mouse button.
        LButton = 1,
        //
        // 요약:
        //     The right mouse button.
        RButton = 2,
        //
        // 요약:
        //     The CANCEL key.
        Cancel = 3,
        //
        // 요약:
        //     The middle mouse button (three-button mouse).
        MButton = 4,
        //
        // 요약:
        //     The first x mouse button (five-button mouse).
        XButton1 = 5,
        //
        // 요약:
        //     The second x mouse button (five-button mouse).
        XButton2 = 6,
        //
        // 요약:
        //     The BACKSPACE key.
        Back = 8,
        //
        // 요약:
        //     The TAB key.
        Tab = 9,
        //
        // 요약:
        //     The LINEFEED key.
        LineFeed = 10,
        //
        // 요약:
        //     The CLEAR key.
        Clear = 12,
        //
        // 요약:
        //     The RETURN key.
        Return = 13,
        //
        // 요약:
        //     The ENTER key.
        Enter = 13,
        //
        // 요약:
        //     The SHIFT key.
        ShiftKey = 16,
        //
        // 요약:
        //     The CTRL key.
        ControlKey = 17,
        //
        // 요약:
        //     The ALT key.
        Menu = 18,
        //
        // 요약:
        //     The PAUSE key.
        Pause = 19,
        //
        // 요약:
        //     The CAPS LOCK key.
        Capital = 20,
        //
        // 요약:
        //     The CAPS LOCK key.
        CapsLock = 20,
        //
        // 요약:
        //     The IME Kana mode key.
        KanaMode = 21,
        //
        // 요약:
        //     The IME Hanguel mode key. (maintained for compatibility; use HangulMode)
        HanguelMode = 21,
        //
        // 요약:
        //     The IME Hangul mode key.
        HangulMode = 21,
        //
        // 요약:
        //     The IME Junja mode key.
        JunjaMode = 23,
        //
        // 요약:
        //     The IME final mode key.
        FinalMode = 24,
        //
        // 요약:
        //     The IME Hanja mode key.
        HanjaMode = 25,
        //
        // 요약:
        //     The IME Kanji mode key.
        KanjiMode = 25,
        //
        // 요약:
        //     The ESC key.
        Escape = 27,
        //
        // 요약:
        //     The IME convert key.
        IMEConvert = 28,
        //
        // 요약:
        //     The IME nonconvert key.
        IMENonconvert = 29,
        //
        // 요약:
        //     The IME accept key, replaces System.Windows.Forms.Keys.IMEAceept.
        IMEAccept = 30,
        //
        // 요약:
        //     The IME accept key. Obsolete, use System.Windows.Forms.Keys.IMEAccept instead.
        IMEAceept = 30,
        //
        // 요약:
        //     The IME mode change key.
        IMEModeChange = 31,
        //
        // 요약:
        //     The SPACEBAR key.
        Space = 32,
        //
        // 요약:
        //     The PAGE UP key.
        Prior = 33,
        //
        // 요약:
        //     The PAGE UP key.
        PageUp = 33,
        //
        // 요약:
        //     The PAGE DOWN key.
        Next = 34,
        //
        // 요약:
        //     The PAGE DOWN key.
        PageDown = 34,
        //
        // 요약:
        //     The END key.
        End = 35,
        //
        // 요약:
        //     The HOME key.
        Home = 36,
        //
        // 요약:
        //     The LEFT ARROW key.
        Left = 37,
        //
        // 요약:
        //     The UP ARROW key.
        Up = 38,
        //
        // 요약:
        //     The RIGHT ARROW key.
        Right = 39,
        //
        // 요약:
        //     The DOWN ARROW key.
        Down = 40,
        //
        // 요약:
        //     The SELECT key.
        Select = 41,
        //
        // 요약:
        //     The PRINT key.
        Print = 42,
        //
        // 요약:
        //     The EXECUTE key.
        Execute = 43,
        //
        // 요약:
        //     The PRINT SCREEN key.
        Snapshot = 44,
        //
        // 요약:
        //     The PRINT SCREEN key.
        PrintScreen = 44,
        //
        // 요약:
        //     The INS key.
        Insert = 45,
        //
        // 요약:
        //     The DEL key.
        Delete = 46,
        //
        // 요약:
        //     The HELP key.
        Help = 47,
        //
        // 요약:
        //     The 0 key.
        D0 = 48,
        //
        // 요약:
        //     The 1 key.
        D1 = 49,
        //
        // 요약:
        //     The 2 key.
        D2 = 50,
        //
        // 요약:
        //     The 3 key.
        D3 = 51,
        //
        // 요약:
        //     The 4 key.
        D4 = 52,
        //
        // 요약:
        //     The 5 key.
        D5 = 53,
        //
        // 요약:
        //     The 6 key.
        D6 = 54,
        //
        // 요약:
        //     The 7 key.
        D7 = 55,
        //
        // 요약:
        //     The 8 key.
        D8 = 56,
        //
        // 요약:
        //     The 9 key.
        D9 = 57,
        //
        // 요약:
        //     The A key.
        A = 65,
        //
        // 요약:
        //     The B key.
        B = 66,
        //
        // 요약:
        //     The C key.
        C = 67,
        //
        // 요약:
        //     The D key.
        D = 68,
        //
        // 요약:
        //     The E key.
        E = 69,
        //
        // 요약:
        //     The F key.
        F = 70,
        //
        // 요약:
        //     The G key.
        G = 71,
        //
        // 요약:
        //     The H key.
        H = 72,
        //
        // 요약:
        //     The I key.
        I = 73,
        //
        // 요약:
        //     The J key.
        J = 74,
        //
        // 요약:
        //     The K key.
        K = 75,
        //
        // 요약:
        //     The L key.
        L = 76,
        //
        // 요약:
        //     The M key.
        M = 77,
        //
        // 요약:
        //     The N key.
        N = 78,
        //
        // 요약:
        //     The O key.
        O = 79,
        //
        // 요약:
        //     The P key.
        P = 80,
        //
        // 요약:
        //     The Q key.
        Q = 81,
        //
        // 요약:
        //     The R key.
        R = 82,
        //
        // 요약:
        //     The S key.
        S = 83,
        //
        // 요약:
        //     The T key.
        T = 84,
        //
        // 요약:
        //     The U key.
        U = 85,
        //
        // 요약:
        //     The V key.
        V = 86,
        //
        // 요약:
        //     The W key.
        W = 87,
        //
        // 요약:
        //     The X key.
        X = 88,
        //
        // 요약:
        //     The Y key.
        Y = 89,
        //
        // 요약:
        //     The Z key.
        Z = 90,
        //
        // 요약:
        //     The left Windows logo key (Microsoft Natural Keyboard).
        LWin = 91,
        //
        // 요약:
        //     The right Windows logo key (Microsoft Natural Keyboard).
        RWin = 92,
        //
        // 요약:
        //     The application key (Microsoft Natural Keyboard).
        Apps = 93,
        //
        // 요약:
        //     The computer sleep key.
        Sleep = 95,
        //
        // 요약:
        //     The 0 key on the numeric keypad.
        NumPad0 = 96,
        //
        // 요약:
        //     The 1 key on the numeric keypad.
        NumPad1 = 97,
        //
        // 요약:
        //     The 2 key on the numeric keypad.
        NumPad2 = 98,
        //
        // 요약:
        //     The 3 key on the numeric keypad.
        NumPad3 = 99,
        //
        // 요약:
        //     The 4 key on the numeric keypad.
        NumPad4 = 100,
        //
        // 요약:
        //     The 5 key on the numeric keypad.
        NumPad5 = 101,
        //
        // 요약:
        //     The 6 key on the numeric keypad.
        NumPad6 = 102,
        //
        // 요약:
        //     The 7 key on the numeric keypad.
        NumPad7 = 103,
        //
        // 요약:
        //     The 8 key on the numeric keypad.
        NumPad8 = 104,
        //
        // 요약:
        //     The 9 key on the numeric keypad.
        NumPad9 = 105,
        //
        // 요약:
        //     The multiply key.
        Multiply = 106,
        //
        // 요약:
        //     The add key.
        Add = 107,
        //
        // 요약:
        //     The separator key.
        Separator = 108,
        //
        // 요약:
        //     The subtract key.
        Subtract = 109,
        //
        // 요약:
        //     The decimal key.
        Decimal = 110,
        //
        // 요약:
        //     The divide key.
        Divide = 111,
        //
        // 요약:
        //     The F1 key.
        F1 = 112,
        //
        // 요약:
        //     The F2 key.
        F2 = 113,
        //
        // 요약:
        //     The F3 key.
        F3 = 114,
        //
        // 요약:
        //     The F4 key.
        F4 = 115,
        //
        // 요약:
        //     The F5 key.
        F5 = 116,
        //
        // 요약:
        //     The F6 key.
        F6 = 117,
        //
        // 요약:
        //     The F7 key.
        F7 = 118,
        //
        // 요약:
        //     The F8 key.
        F8 = 119,
        //
        // 요약:
        //     The F9 key.
        F9 = 120,
        //
        // 요약:
        //     The F10 key.
        F10 = 121,
        //
        // 요약:
        //     The F11 key.
        F11 = 122,
        //
        // 요약:
        //     The F12 key.
        F12 = 123,
        //
        // 요약:
        //     The F13 key.
        F13 = 124,
        //
        // 요약:
        //     The F14 key.
        F14 = 125,
        //
        // 요약:
        //     The F15 key.
        F15 = 126,
        //
        // 요약:
        //     The F16 key.
        F16 = 127,
        //
        // 요약:
        //     The F17 key.
        F17 = 128,
        //
        // 요약:
        //     The F18 key.
        F18 = 129,
        //
        // 요약:
        //     The F19 key.
        F19 = 130,
        //
        // 요약:
        //     The F20 key.
        F20 = 131,
        //
        // 요약:
        //     The F21 key.
        F21 = 132,
        //
        // 요약:
        //     The F22 key.
        F22 = 133,
        //
        // 요약:
        //     The F23 key.
        F23 = 134,
        //
        // 요약:
        //     The F24 key.
        F24 = 135,
        //
        // 요약:
        //     The NUM LOCK key.
        NumLock = 144,
        //
        // 요약:
        //     The SCROLL LOCK key.
        Scroll = 145,
        //
        // 요약:
        //     The left SHIFT key.
        LShiftKey = 160,
        //
        // 요약:
        //     The right SHIFT key.
        RShiftKey = 161,
        //
        // 요약:
        //     The left CTRL key.
        LControlKey = 162,
        //
        // 요약:
        //     The right CTRL key.
        RControlKey = 163,
        //
        // 요약:
        //     The left ALT key.
        LMenu = 164,
        //
        // 요약:
        //     The right ALT key.
        RMenu = 165,
        //
        // 요약:
        //     The browser back key.
        BrowserBack = 166,
        //
        // 요약:
        //     The browser forward key.
        BrowserForward = 167,
        //
        // 요약:
        //     The browser refresh key.
        BrowserRefresh = 168,
        //
        // 요약:
        //     The browser stop key.
        BrowserStop = 169,
        //
        // 요약:
        //     The browser search key.
        BrowserSearch = 170,
        //
        // 요약:
        //     The browser favorites key.
        BrowserFavorites = 171,
        //
        // 요약:
        //     The browser home key.
        BrowserHome = 172,
        //
        // 요약:
        //     The volume mute key.
        VolumeMute = 173,
        //
        // 요약:
        //     The volume down key.
        VolumeDown = 174,
        //
        // 요약:
        //     The volume up key.
        VolumeUp = 175,
        //
        // 요약:
        //     The media next track key.
        MediaNextTrack = 176,
        //
        // 요약:
        //     The media previous track key.
        MediaPreviousTrack = 177,
        //
        // 요약:
        //     The media Stop key.
        MediaStop = 178,
        //
        // 요약:
        //     The media play pause key.
        MediaPlayPause = 179,
        //
        // 요약:
        //     The launch mail key.
        LaunchMail = 180,
        //
        // 요약:
        //     The select media key.
        SelectMedia = 181,
        //
        // 요약:
        //     The start application one key.
        LaunchApplication1 = 182,
        //
        // 요약:
        //     The start application two key.
        LaunchApplication2 = 183,
        //
        // 요약:
        //     The OEM Semicolon key on a US standard keyboard.
        OemSemicolon = 186,
        //
        // 요약:
        //     The OEM 1 key.
        Oem1 = 186,
        //
        // 요약:
        //     The OEM plus key on any country/region keyboard.
        Oemplus = 187,
        //
        // 요약:
        //     The OEM comma key on any country/region keyboard.
        Oemcomma = 188,
        //
        // 요약:
        //     The OEM minus key on any country/region keyboard.
        OemMinus = 189,
        //
        // 요약:
        //     The OEM period key on any country/region keyboard.
        OemPeriod = 190,
        //
        // 요약:
        //     The OEM question mark key on a US standard keyboard.
        OemQuestion = 191,
        //
        // 요약:
        //     The OEM 2 key.
        Oem2 = 191,
        //
        // 요약:
        //     The OEM tilde key on a US standard keyboard.
        Oemtilde = 192,
        //
        // 요약:
        //     The OEM 3 key.
        Oem3 = 192,
        //
        // 요약:
        //     The OEM open bracket key on a US standard keyboard.
        OemOpenBrackets = 219,
        //
        // 요약:
        //     The OEM 4 key.
        Oem4 = 219,
        //
        // 요약:
        //     The OEM pipe key on a US standard keyboard.
        OemPipe = 220,
        //
        // 요약:
        //     The OEM 5 key.
        Oem5 = 220,
        //
        // 요약:
        //     The OEM close bracket key on a US standard keyboard.
        OemCloseBrackets = 221,
        //
        // 요약:
        //     The OEM 6 key.
        Oem6 = 221,
        //
        // 요약:
        //     The OEM singled/double quote key on a US standard keyboard.
        OemQuotes = 222,
        //
        // 요약:
        //     The OEM 7 key.
        Oem7 = 222,
        //
        // 요약:
        //     The OEM 8 key.
        Oem8 = 223,
        //
        // 요약:
        //     The OEM angle bracket or backslash key on the RT 102 key keyboard.
        OemBackslash = 226,
        //
        // 요약:
        //     The OEM 102 key.
        Oem102 = 226,
        //
        // 요약:
        //     The PROCESS KEY key.
        ProcessKey = 229,
        //
        // 요약:
        //     Used to pass Unicode characters as if they were keystrokes. The Packet key value
        //     is the low word of a 32-bit virtual-key value used for non-keyboard input methods.
        Packet = 231,
        //
        // 요약:
        //     The ATTN key.
        Attn = 246,
        //
        // 요약:
        //     The CRSEL key.
        Crsel = 247,
        //
        // 요약:
        //     The EXSEL key.
        Exsel = 248,
        //
        // 요약:
        //     The ERASE EOF key.
        EraseEof = 249,
        //
        // 요약:
        //     The PLAY key.
        Play = 250,
        //
        // 요약:
        //     The ZOOM key.
        Zoom = 251,
        //
        // 요약:
        //     A constant reserved for future use.
        NoName = 252,
        //
        // 요약:
        //     The PA1 key.
        Pa1 = 253,
        //
        // 요약:
        //     The CLEAR key.
        OemClear = 254,
        //
        // 요약:
        //     The bitmask to extract a key code from a key value.
        KeyCode = 65535,
        //
        // 요약:
        //     The SHIFT modifier key.
        Shift = 65536,
        //
        // 요약:
        //     The CTRL modifier key.
        Control = 131072,
        //
        // 요약:
        //     The ALT modifier key.
        Alt = 262144
    }

    
}


