using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using EasyMacro.Model;
using EasyMacroAPI;
using EasyMacroAPI.Command;

namespace EasyMacro.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// 매크로 흐름에 대한 정보입니다.
        /// </summary>
        public ObservableCollection<Model.IMacros> LogicList { get; set; } = new();

        private MacroManager macroManager;

        public MainWindowViewModel()
        {
            macroManager = MacroManager.Instance;

            //macroManager.InsertList(new Hello());
            //macroManager.DoOnce(0);
        }

        public ICommand SaveCommand => new RelayCommand(Save);
        public ICommand LoadCommand => new RelayCommand(Load);
        public ICommand StartCommand => new RelayCommand(Start);

        private void Save()
        {
            MessageBox.Show("Save Commend Excuted!");
            // Call save function from library
            macroManager.SaveData();
        }

        private void Load()
        {
            MessageBox.Show("Load Commend Excuted!");
            // Call load function from library
            macroManager.LoadData();
            LogicList.Clear();

            // 아래는 public 가정입니다.
            foreach (IAction i in macroManager.actionList)
            {
                switch (i.MacroType)
                {
                    default:
                        LogicList.Add(new UndefinedMacro(i));
                        break;
                }
            }
        }

        private void Start()
        {
            MessageBox.Show("Start Commend Excuted!");

            //macroManager.DoOnce(0);
            macroManager.StartMacro();
        }

    }
}
