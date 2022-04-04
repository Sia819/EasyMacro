using System;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using EasyMacro.Model;
using EasyMacro.Model.DisplayMacro;
using EasyMacroAPI;
using EasyMacroAPI.Command;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System.Threading;

namespace EasyMacro.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 매크로 흐름에 대한 정보입니다.
        /// </summary>
        public ObservableCollection<Model.IMacros> LogicList { get; set; } = new();
        
        /// <summary>
        /// 디자이너에서만 보여질 리스트이며, 디자인 전용으로 만들어졌습니다.
        /// </summary>
        public ObservableCollection<Model.IMacros> DesignerList { get; set; }

        private MacroManager macroManager;

        #region Public Command

        public RelayCommand SaveCommand => new(Save);
        public RelayCommand LoadCommand => new(Load);
        public RelayCommand StartCommand => new(Start);
        public RelayCommand<EasyMacroAPI.Model.IAction> AddCommand => new(Add);
        public RelayCommand TestCommand => new(Test);


        #endregion

        public MainWindowViewModel()
        {
            macroManager = MacroManager.Instance;
            DesignerList = new ObservableCollection<IMacros>()
            {
                new SleepMacro(new EasyMacroAPI.Command.Delay(251)),
                new MouseMacro(new EasyMacroAPI.Command.MouseMove(200, 230)),
                new MouseMacro(new EasyMacroAPI.Command.MouseClick(200, 230)),
            };
        }

        #region Private Command Function

        private void Test()
        {
            MacroManager macroManager = MacroManager.Instance;
            //파일명 넣을것
            //macroManager.InsertList(new EasyMacroAPI.CaptureManager(@"C:\"));
            macroManager.DoOnce(0);
        }

        private void Save()
        {
            MessageBox.Show("Save Commend Excuted!");
            // Call save function from library
            macroManager.SaveData();
        }

        /// <summary>
        /// 매크로 라이브러리에서 
        /// </summary>
        private void Load()
        {
            string macroPath = null;
            if (!Common.DebugState.IsDebugStart)
            {
                OpenFileDialog dlgOpenFile = new OpenFileDialog();
                dlgOpenFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);        // OpenFileDialog에서 열리는 첫 페이지는 바탕화면입니다.
                dlgOpenFile.InitialDirectory = "MacroFiles (*.xml, *.em) | *.xml; *.em; | All files (*.*) | *.*";

                if (dlgOpenFile.ShowDialog() == true)
                {// OpenFileDialog에서 경로가 지정 됨.
                    macroPath = dlgOpenFile.FileName;
                }
                else
                {// 취소함
                    MessageBox.Show("파일 열기 작업이 취소되었습니다.");
                }
            }

            macroManager.LoadData(macroPath);
            LogicList.Clear();

            // 아래는 macroManager의 public가정으로 만들어진 코드 입니다.
            // 로드된 매크로를 UI에 바인딩된 리스트에 등록시킵니다.
            foreach (EasyMacroAPI.Model.IAction i in macroManager.actionList)
            {
                switch (i.MacroType)
                {
                    case EasyMacroAPI.Model.MacroTypes.Delay:
                        break;
                    case EasyMacroAPI.Model.MacroTypes.MouseMove:
                        break;
                    case EasyMacroAPI.Model.MacroTypes.MouseClick:
                        break;
                    default:
                        LogicList.Add(new UndefinedMacro(i));
                        break;
                }
            }
        }

        /// <summary>
        /// 매크로 편집기에서 새로운 매크로를 추가합니다.
        /// </summary>
        /// <param name="action"></param>
        private void Add(EasyMacroAPI.Model.IAction action)
        {

        }

        /// <summary>
        /// 리스트로 로드된 모든 매크로를 실행합니다.
        /// </summary>
        private void Start()
        {
            MessageBox.Show("Start Commend Excuted!");

            //macroManager.DoOnce(0);
            macroManager.StartMacro();
        }

        #endregion

    }
}
