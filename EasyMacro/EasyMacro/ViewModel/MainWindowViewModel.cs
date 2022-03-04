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
        public MacroManager macroManager;

        public MainWindowViewModel()
        {
            macroManager = MacroManager.Instance;
            MacroList.Add(new MacrosCommand("Hello"));
            macroManager.InsertList(new Hello());
        }

        public ICommand SaveCommand { get; } = new RelayCommand(() => 
        {
            MacroManager.Instance.SaveData();
            MessageBox.Show("Save Commend!");
        });

        public ICommand LoadCommand { get; } = new RelayCommand(() =>
        {
            MessageBox.Show("Load Commend!");
        });
    }
}
