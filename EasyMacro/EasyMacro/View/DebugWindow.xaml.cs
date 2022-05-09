using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ReactiveUI;
using EasyMacro.ViewModel;
using System.Reactive.Disposables;

namespace EasyMacro.View
{
    /// <summary>
    /// DebugWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DebugWindow : Window
    {
        public DebugWindow()
        {
            InitializeComponent();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO : 애니메이션 안보임
            Window.GetWindow(this).WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            Window targetWindow = Window.GetWindow(this);
            _ = targetWindow.WindowState == WindowState.Normal ?
                targetWindow.WindowState = WindowState.Maximized :
                targetWindow.WindowState = WindowState.Normal;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
