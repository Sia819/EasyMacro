using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using EasyMacro.Model;
using EasyMacro.Common;
using EasyMacroAPI;
using EasyMacroAPI.Command;
using Microsoft.Win32;

namespace EasyMacro.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// 매크로 흐름에 대한 정보입니다.
        /// </summary>
        public ObservableCollection<Model.IMacros> LogicList { get; set; } = new();

        private MacroManager macroManager;

        #region Public Command

        public RelayCommand SaveCommand => new(Save);
        public RelayCommand LoadCommand => new(Load);
        public RelayCommand StartCommand => new(Start);
        public RelayCommand<EasyMacroAPI.Model.IAction> AddCommand => new(Add);

        #endregion

        public MainWindowViewModel()
        {
            macroManager = MacroManager.Instance;

            //macroManager.InsertList(new Hello());
            //macroManager.DoOnce(0);
        }



        #region Private Command Function

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
                OpenFileDialog dlgOpenFile = new OpenFileDialog()
                {
                    Filter = "Image Files (*.xml, *.em) | *.xml; *.em; | All files (*.*) | *.*",
                };
                if (dlgOpenFile.ShowDialog().ToString() == "OK")
                {
                    macroPath = dlgOpenFile.FileName;
                }
                else
                {
                    MessageBox.Show("파일 열기 작업이 취소되었습니다.");
                }
            }

            macroManager.LoadData(macroPath);
            LogicList.Clear();

            // 아래는 public 가정입니다.
            foreach (EasyMacroAPI.Model.IAction i in macroManager.actionList)
            {
                switch (i.MacroType)
                {
                    default:
                        LogicList.Add(new UndefinedMacro(i));
                        break;
                }
            }
        }

        private void Add(EasyMacroAPI.Model.IAction action)
        {

        }

        private void Start()
        {
            MessageBox.Show("Start Commend Excuted!");

            //macroManager.DoOnce(0);
            macroManager.StartMacro();
        }

        #endregion

    }
}
