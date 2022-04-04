using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Xml;
using EasyMacroAPI.Command;
using EasyMacroAPI.CommandSerializer;
using EasyMacroAPI.Common;
using EasyMacroAPI.Model;
using ExtendedXmlSerializer;
using ExtendedXmlSerializer.ExtensionModel;
using ExtendedXmlSerializer.Configuration;
using System.Drawing;

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

        private MessageReceiver hotKey;

        private bool isMacroStarted;

        private Thread macroThread;

        /// <summary>
        /// 바탕화면 주소입니다.
        /// </summary>
        private string deaktopPath;

        /// <summary>
        /// 저장될 파일명 이름입니다.
        /// </summary>
        private string saveFileName;

        /// <summary>
        /// 직렬화 객체입니다. <para/>
        /// https://github.com/ExtendedXmlSerializer/home 사이트 참고
        /// </summary>
        private IExtendedXmlSerializer serializer;

        /// <summary>
        /// 커스텀 직렬화 객체입니다.
        /// </summary>
        private MacroCustomSerializer customSerializer;


        public Point tempPoint;

        #endregion

        private MacroManager()
        {
            
            new Thread(new ThreadStart(() =>
            {
                // Thread do
                hotKey = new MessageReceiver(); // TODO : Console실행시 Error발생
                hotKey.Run();

            }))
            {
                // Thread Properties
                IsBackground = true
            }
            .Start();

            actionList = new List<IAction>();
            isMacroStarted = false;
            deaktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileName = "test.xml";

            // 시리얼라이저 객체 초기화 부분 입니다.
            this.customSerializer = new MacroCustomSerializer();
            customSerializer.Register<Delay>(new DelaySerializer())
                            .Register<MouseMove>(new MouseMoveSerializer());
            serializer = new ConfigurationContainer().Type<IAction>().CustomSerializer(customSerializer)
                                                     //.CustomSerializer<Delay, DelaySerializer>()
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
            hotKey.AddHotkey(Keys.F9, KeyModifiers.None, StopMacro);
            macroThread.Start();
        }

        public void StopMacro()
        {
            isMacroStarted = false;
            hotKey.RemoveHotkey(Keys.F9, KeyModifiers.None);
            Console.WriteLine("Stopped");
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

        /// <summary>
        /// 현재 매크로 리스트의 모든 내용을 파일로 저장합니다.
        /// </summary>
        public void SaveData(string filePath = null)
        {
            if (filePath == null)
            {
                filePath = $"{deaktopPath}\\{saveFileName}";
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
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
                           .Type<IAction>().CustomSerializer(customSerializer)
                           .Create();
                actionList = subject.Deserialize<List<IAction>>(reader);
            }
        }
    }
}