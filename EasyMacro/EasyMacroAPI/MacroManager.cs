using System;
using System.Collections;
using System.Collections.Generic;

namespace EasyMacroAPI
{
    public class MacroManager
    {
        private static MacroManager instance = null;

        private ArrayList arrayList;

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
