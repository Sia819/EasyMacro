namespace EasyMacro.View
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using MahApps.Metro.Controls;

    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            HookLib.GlobalKeyboardHook globalKeyboardHook = new HookLib.GlobalKeyboardHook();
            EasyMacroAPI.MacroManager.Instance.RegisterMessageReceiver(globalKeyboardHook);
            HookLib.GlobalKeyboardHook.StartKeyboardHook();

            
            HookLib.GlobalMouseKeyHook.SetDispatcher(this.Dispatcher);
            
        }

        


        private void mainFrame_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateFrameDataContext(sender, null);
        }

        private void mainFrame_LoadCompleted(object sender, NavigationEventArgs e)
        {
            UpdateFrameDataContext(sender, e);
        }

        private void UpdateFrameDataContext(object sender, NavigationEventArgs e)
        {

            var content = (sender as Frame).Content as FrameworkElement;
            if (content == null)
                return;
            content.DataContext = (sender as Frame).DataContext;

        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;

            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    this.mainFrame.Source = new Uri("pack://application:,,,/View/ListEditPage.xaml", UriKind.Absolute);
                }
                else
                {
                    this.mainFrame.Source = new Uri("pack://application:,,,/View/NodeEditPage.xaml", UriKind.Absolute);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }
    }

    public partial class MainWindow : MetroWindow
    {
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
        }
    }
}