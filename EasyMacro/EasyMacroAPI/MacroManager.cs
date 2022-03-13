using System;
using System.Collections.Generic;
using System.Threading;

namespace EasyMacroAPI
{
    public class MacroManager
    {
        private static MacroManager instance;
        private HotKey hotKey;
        private bool isMacroStarted;
        private Thread macroThread;
        private IOManager ioManger;
        // TODO : 임시로 private -> public 수정 나중에 고치기
        public List<IAction> actionList;

        private MacroManager()
        {
            ioManger = new IOManager();
            hotKey = new HotKey();
            actionList = new List<IAction>();
            isMacroStarted = false;
        }

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


        public void InsertList(IAction insertAction)
        {
            actionList.Add(insertAction);
        }
        
        public void DeleteList(int index)
        {
            actionList.RemoveAt(index);
        }

        public void StartMacro()
        {
            macroThread = new Thread(DoMacro);
            macroThread.IsBackground = true;
            isMacroStarted = true;
            hotKey.RegisterHotKey();
            macroThread.Start();
        }

        public void DelayMacro(int time)
        {
            Thread.Sleep(time);
        }

        public void StopMacro()
        {
            isMacroStarted = false;
            hotKey.UnregisterHotKey();
        }

        private void DoMacro()
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

        public void DoOnce(int index)
        {
            actionList[index].Do();
        }

        public void SaveData(List<IAction> list)
        {
            ioManger.Serialization(list);
        }

        public void LoadData()
        {
            actionList = ioManger.DeSerialization<List<IAction>>();
        }
    }
}
