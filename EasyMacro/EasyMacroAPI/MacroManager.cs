using System;
using System.Collections.Generic;
using System.Threading;

namespace EasyMacroAPI
{
    public class MacroManager
    {
        private static MacroManager instance = null;

        private static List<ActionAbstract> actionList;
        private static bool isMacroStarted = false;
        static Thread macroThread = new Thread(DoMacro);
        public static MacroManager Instance
        {
            get 
            {
                if (instance == null)
                {
                    Init();
                    instance = new MacroManager();
                }
                return instance;
            }
        }

        private static void Init()
        {
            macroThread.IsBackground = true;
        }

        public void StartMacro()
        {
            isMacroStarted = true;
            macroThread.Start();
        }

        public void StopMacro()
        {
            isMacroStarted = false;
            macroThread.Join();
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
