using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;

namespace EasyMacro.Common
{
    public class ViewModelLocator
    {
        public ViewModel.MainWindowViewModel MainViewModelLocate => 
            SimpleIoc.Default.GetInstance<ViewModel.MainWindowViewModel>();

        public ViewModel.PropertiesEditorViewModel EditorPropertiesViewModelLocate =>
            SimpleIoc.Default.GetInstance<ViewModel.PropertiesEditorViewModel>();

        public ViewModelLocator()
        {
            //MainWindowViewModel and sub viewmodels
            SimpleIoc.Default.Register<ViewModel.MainWindowViewModel>();

            // EditorPropertiesViewModel and sub viewmodels
            SimpleIoc.Default.Register<ViewModel.PropertiesEditorViewModel>();
            SimpleIoc.Default.Register<ViewModel.HelloPropertiesViewModel>();
            SimpleIoc.Default.Register<ViewModel.SleepPropertiesViewModel>();
        }
    }
}
