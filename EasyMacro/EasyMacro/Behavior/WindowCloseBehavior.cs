using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace EasyMacro.Behavior
{
    public class WindowCloseBehavior : Behavior<Button>
    {
        protected override void OnAttached() 
        { 
            base.OnAttached(); 
            AssociatedObject.Loaded += WindowClose_Behavior;
        }
        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= WindowClose_Behavior;
            base.OnDetaching();
        }

        private void WindowClose_Behavior(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
            //Window.GetWindow(sender).Close();
        }

        public static readonly DependencyProperty SalutationProperty =
            DependencyProperty.RegisterAttached("Salutation",
                                                typeof(string),
                                                typeof(WindowCloseBehavior),
                                                new PropertyMetadata(OnEvent));


        private static void OnEvent(object sender, DependencyPropertyChangedEventArgs e)
        {
            //attach to event handlers (Click, Loaded, etc...)
        }
    }
}
