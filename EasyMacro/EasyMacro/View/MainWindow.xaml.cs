namespace EasyMacro.View
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using MahApps.Metro.Controls;
    using EasyMacro.ViewModel;
    using NodeNetwork.ViewModels;

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
            this.mainFrame.Source = new Uri("pack://application:,,,/View/NodeEditPage.xaml", UriKind.Absolute);
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

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            PageViewModel.Instance.Save();
        }

        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            PageViewModel.Instance.Load();
        }


        // private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        // {
        //     ToggleSwitch toggleSwitch = sender as ToggleSwitch;
        // 
        //     if (toggleSwitch != null)
        //     {
        //         if (toggleSwitch.IsOn == true)
        //         {
        //             this.mainFrame.Source = new Uri("pack://application:,,,/View/ListEditPage.xaml", UriKind.Absolute);
        //         }
        //         else
        //         {
        //             this.mainFrame.Source = new Uri("pack://application:,,,/View/NodeEditPage.xaml", UriKind.Absolute);
        //         }
        //     }
        // }
    }

    public partial class MainWindow : MetroWindow
    {
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
        }
    }
}