using System;
using System.Collections.Generic;
using System.Threading;

namespace EasyMacroAPI
{
    public class MacroManager
    {
        private static MacroManager instance = null;

        private static List<ActionAbstaract> actionList;
        private static bool isMacroStarted = false;
        Thread macroThread = new Thread(DoMacro);
        public static MacroManager Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new MacroManager();
                }
                return instance;
            }
        }

        public void StartMacro()
        {
            macroThread.Start();
        }

        private static void DoMacro()
        {
            while (true)
            {
                for (int i = 0; i < actionList.Count; i++)
                {
                    if (isMacroStarted)
                        actionList[i].Do();
                    else
                        return;
                }
            }
        }
    }
}
