using System;
using System.Collections;

namespace EasyMacroAPI
{
    public class MacroManager
    {
        private static MacroManager instance = null;

        public static MacroManager Instance()
        {
            if(instance == null)
            {
                instance = new MacroManager();
            }
            return instance;
        }
    }
}
