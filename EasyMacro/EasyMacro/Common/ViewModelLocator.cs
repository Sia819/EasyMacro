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
        public ViewModel.MainWindowViewModel MainViewModelLocate 
        { 
            get => SimpleIoc.Default.GetInstance<ViewModel.MainWindowViewModel>(); 
        }

        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<ViewModel.MainWindowViewModel>();

        }
    }
}
