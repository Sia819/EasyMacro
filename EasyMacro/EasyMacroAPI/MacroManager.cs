using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using EasyMacroAPI.Command;
using EasyMacroAPI.CommandSerializer;
using EasyMacroAPI.Common;
using ExtendedXmlSerializer;
using ExtendedXmlSerializer.Configuration;

namespace EasyMacroAPI
{
    public class MacroManager
    {
        
        // TODO : 임시로 private -> public 수정 나중에 고치기
        public List<IAction> actionList;

        #region Private Field

        /// <summary>
        /// 싱글톤 객체 입니다.
        /// </summary>
        private static MacroManager instance;

        private HotKey hotKey;

        private bool isMacroStarted;

        private Thread macroThread;

        private IOManager ioManger;

        /// <summary>
        /// 바탕화면 주소입니다.
        /// </summary>
        private string deaktopPath;

        /// <summary>
        /// 저장될 파일명 이름입니다.
        /// </summary>
        private string saveFileName;

        /// <summary>
        /// 직렬화 객체입니다. </para>
        /// https://github.com/ExtendedXmlSerializer/home 사이트 참고
        /// </summary>
        private IExtendedXmlSerializer serializer;

        #endregion

        private MacroManager()
        {
            ioManger = new IOManager(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "macro.em");
            //hotKey = new HotKey();
            actionList = new List<IAction>();
            isMacroStarted = false;
            deaktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileName = "test.xml";
            serializer = new ConfigurationContainer().CustomSerializer<MouseMove, MouseMoveSerializer>()
                                                     .CustomSerializer<Delay, DelaySerializer>()
                                                     .Create();
        }

        public static MacroManager Instance => instance ??= new MacroManager();

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

        /// <summary>
        /// 특정 인덱스의 매크로를 실행합니다.
        /// </summary>
        /// <param name="index">실행할 리스트의 인덱스 입니다.</param>
        public void DoOnce(int index)
        {
            actionList[index].Do();
        }

        public void SaveData()
        {
            string xmlData = serializer.Serialize(actionList);

            using (XmlTextWriter wr = new XmlTextWriter($"{deaktopPath}\\{saveFileName}", Encoding.UTF8))
            {
                wr.Formatting = Formatting.Indented;
                XmlDocument document = new XmlDocument();
                document.LoadXml(xmlData);
                document.WriteContentTo(wr);
                wr.Flush();
            }
        }

        public void LoadData(string filePath = null)
        {
            if (filePath == null)
            {
                filePath = $"{deaktopPath}\\{saveFileName}";
            }
            actionList.Clear();
            using (var reader = new StreamReader(filePath))
            {
                var subject = new ConfigurationContainer()
                           .WithUnknownContent()
                           .Continue()
                           .CustomSerializer<MouseMove>(typeof(MouseMoveSerializer))
                           .Create();
                actionList = subject.Deserialize<List<IAction>>(reader);
            }
        }
    }
}
