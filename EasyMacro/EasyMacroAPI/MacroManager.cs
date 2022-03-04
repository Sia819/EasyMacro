using System;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace EasyMacroAPI
{
    public class MacroManager
    {
        private static MacroManager instance;
        private bool isMacroStarted;
        private Thread macroThread;
        // TODO : 임시로 private -> public 수정 나중에 고치기
        public List<IAction> actionList;

        private MacroManager()
        {
            actionList = new List<IAction>();
            isMacroStarted = false;
            macroThread = new Thread(DoMacro);
            macroThread.IsBackground = true;
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
            isMacroStarted = true;
            macroThread.Start();
        }

        public void DelayMacro(int time)
        {
            Thread.Sleep(time);
        }

        public void StopMacro()
        {
            isMacroStarted = false;
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

        public void SaveData()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream("C:\\Users\\Sia819\\Desktop\\test\\macro.em", FileMode.Create);

            SerializableDataField filesaver = new SerializableDataField();

            filesaver.copyList = actionList;
            bf.Serialize(fs, filesaver);
            fs.Close();
        }

        public void LoadData()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream("C:\\Users\\Sia819\\Desktop\\test\\macro.em", FileMode.Open);

            SerializableDataField filesaver = new SerializableDataField();

            filesaver = bf.Deserialize(fs) as SerializableDataField;
            fs.Close();

            actionList = filesaver.copyList;
        }
    }
}
