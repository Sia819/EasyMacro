using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EasyMacro.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using EasyMacroAPI;

namespace EasyMacro.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Model.MacrosCommand> MacroList { get; set; } = new();

        private MacroManager macroManager;

        public MainWindowViewModel()
        {
            macroManager = MacroManager.Instance;

            MacroList.Add(new MacrosCommand("Hello1"));
            MacroList.Add(new MacrosCommand("Hello2", true));
            MacroList.Add(new MacrosCommand("Hello3"));

            //macroManager.InsertList(new Hello());
            //macroManager.DoOnce(0);
        }

        public ICommand SaveCommand => new RelayCommand(Save);
        public ICommand LoadCommand => new RelayCommand(Load);
        public ICommand StartCommand => new RelayCommand(Start);

        private void Save()
        {
            MessageBox.Show("Save Commend Excuted!");

            macroManager.SaveData();
        }

        private void Load()
        {
            MessageBox.Show("Load Commend Excuted!");

            macroManager.LoadData();
        }

        private void Start()
        {
            MessageBox.Show("Start Commend Excuted!");

            macroManager.DoOnce(0);
        }

    }
}
