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

namespace EasyMacro.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Model.MacrosCommand> MacroList { get; set; } = new();

        public MainWindowViewModel()
        {
            MacroList.Add(new MacrosCommand("Hello"));
        }

        public ICommand SaveCommand { get; } = new RelayCommand(() => 
        {
            MessageBox.Show("Save Commend!");
        });

        public ICommand LoadCommand { get; } = new RelayCommand(() =>
        {
            MessageBox.Show("Load Commend!");
        });
    }
}
