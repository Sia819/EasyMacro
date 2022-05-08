﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasyMacro.View
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //this.mainFrame.Source = new Uri("pack://application:,,,/View/NodeEditPage.xaml");
            this.mainFrame.Source = new Uri("pack://application:,,,/View/ListEditPage.xaml", UriKind.Absolute);
        }

        private void mainFrame_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateFrameDataContext(sender, null);
        }

        private void frame_LoadCompleted(object sender, NavigationEventArgs e)
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

        private void CheckBox_CheckedChange(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
            {
                this.mainFrame.Source = new Uri("pack://application:,,,/View/ListEditPage.xaml", UriKind.Absolute);
            }
            else
            {
                this.mainFrame.Source = new Uri("pack://application:,,,/View/NodeEditPage.xaml", UriKind.Absolute);
            }
        }
    }
}
