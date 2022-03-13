using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using EasyMacro.Model;
using EasyMacroAPI;


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

            macroManager.InsertList(new MouseMove(100, 100));
            macroManager.InsertList(new DelayMacro(1000));
            macroManager.InsertList(new MouseMove(1000, 1000));
            macroManager.InsertList(new DelayMacro(1000));
        }

        public ICommand SaveCommand => new RelayCommand(Save);
        public ICommand LoadCommand => new RelayCommand(Load);
        public ICommand StartCommand => new RelayCommand(Start);
        public ICommand AddCommand => new RelayCommand(Add);

        private void Save()
        {
            MessageBox.Show("Save Commend Excuted!");

            macroManager.SaveData();
        }

        private void Load()
        {
            MessageBox.Show("Load Commend Excuted!");

            macroManager.LoadData();
            LogicList.Clear();

            // 아래는 public 가정입니다.
            foreach (IAction i in macroManager.actionList)
            {
                switch (i.MacroType)
                {
                    case MacroTypes.Hello:
                        LogicList.Add(new HelloMacro(i));
                        break;
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

        /// <summary>
        /// 특정 매크로를 리스트에 추가합니다.
        /// </summary>
        private void Add()
        {
            MacroManager.Instance.InsertList(new MouseMove(100, 100));
            MessageBox.Show("Add Commend Excuted!");
            // TODO : ComboBoxList 들의 ContentPresentor은 각각 ViewModel 이 있는데
            // 그 ViewModel에서 제공하는 모든 옵션들이 다르기에 Add가
            // MainWidnowViewModel 에 있으면 모든 옵션들을 각각 담을 수 없거나
            // 참조가 너무 짙어지는 문제가 있음. -> 각각이 Add함수를 구현하는 방안으로 생각해봐야 될 것임
            // Properties인 UserControl의 ViewModel들을 인터페이스(Add함수구현)으로 규격화하고
            // MainWindowViewModel의 매크로 로직 리스트가 Properties UC ViewModel에 참조당해서 Add 당하는게
            // 합리적이라고 생각함.
            // dp 로 구현하면 좋을듯.

            /*
            switch ()
            {
                case MacroCommand.Hello:
                    LogicList.Add(new HelloMacro(""));
                    break;
                case MacroCommand.Sleep:
                    throw new NotImplementedException();
                    break;
                case MacroCommand.MouseMove:
                    throw new NotImplementedException();
                    break;
                case MacroCommand.MouseClick:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }
            */

        }

    }
}
