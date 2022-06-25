namespace EasyMacro.View
{
    using System.Windows;
    using MahApps.Metro.Controls;
    using EasyMacro.ViewModel;
    using System.Windows.Media;

    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Source = new Uri("pack://application:,,,/View/NodeEditPage.xaml", UriKind.Absolute);
        }

        /// <summary> Save button in title bar clicked </summary>
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            PageViewModel.Instance.Save();
        }

        /// <summary> Load button in title clicked </summary>
        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            PageViewModel.Instance.Load();
        }

        /// <summary> Night toggle button in title clicked </summary>
        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleSwitch toggleSwitch)
            {
                if (toggleSwitch.IsOn)
                {
                    this.Background = new SolidColorBrush(Color.FromRgb(54, 57, 63));
                }
                else
                {
                    this.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                }
            }
        }
    }
}