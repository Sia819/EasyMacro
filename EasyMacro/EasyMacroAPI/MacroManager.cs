using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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

        public void InsertList(ActionAbstract insertAction)
        {
            insertAction.index = actionList.Count;
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

        public static void DoOnce(int index)
        {
            actionList[index].Do();
        }

        public void SaveData()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream("C:\\emsave.em", FileMode.Create);

            SerializableDataField filesaver = new SerializableDataField();

            filesaver.actionList = actionList;

            bf.Serialize(fs, filesaver);
            fs.Close();
        }

        public void LoadData()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream("C:\\emsave.em", FileMode.Open);

            SerializableDataField filesaver = new SerializableDataField();

            filesaver = bf.Deserialize(fs) as SerializableDataField;
            fs.Close();

            actionList = filesaver.actionList;
        }
    }
}
