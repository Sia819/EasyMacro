using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace EasyMacroAPI
{
    public class MacroManager
    {
        private static MacroManager instance = null;

        private List<ActionAbstaract> actionList;
        Thread macroThread;
        public static MacroManager Instance()
        {
            if(instance == null)
            {
                instance = new MacroManager();
            }
            return instance;
        }

        public void StartMacro()
        {
            
        }

        private void DoMacro()
        {
            for (int i = 0; i < actionList.Count; i++)
            {

                actionList[i].Do();
            }
        }
    }
}
