using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;

namespace EasyMacro.Common
{
    /// <summary>
    /// View의 DataContext를 ViewModel의 인스턴스에 연결하는 데 사용됩니다.
    /// </summary>
    public class ViewModelLocator
    {
        public ViewModel.MainWindowViewModel MainViewModelLocate => SimpleIoc.Default.GetInstance<ViewModel.MainWindowViewModel>();

        public ViewModel.PageViewModel PageViewModelLocate => SimpleIoc.Default.GetInstance<ViewModel.PageViewModel>();

        public ViewModelLocator()
        {
            // MainWindowViewModel and sub viewmodels
            SimpleIoc.Default.Register<ViewModel.MainWindowViewModel>();
            SimpleIoc.Default.Register<ViewModel.PageViewModel>();
        }
    }
}
