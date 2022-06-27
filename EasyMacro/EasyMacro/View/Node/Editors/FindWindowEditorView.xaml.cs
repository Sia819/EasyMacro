using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EasyMacro.ViewModel.Node.Editors;
using ReactiveUI;
using PInvoke;
using System.Runtime.InteropServices;
using System.Collections;
using System.Windows.Interop;
using System.Text;

namespace EasyMacro.View.Node.Editors
{
    /// <summary>
    /// FindWindowEditor.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FindWindowEditorView : UserControl, IViewFor<FindWindowEditorViewModel>
    {
        #region DllImport

        [DllImport("user32.dll")]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        #endregion

        #region Mode - DP
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(bool), typeof(FindWindowEditorView));
        public bool Mode
        {
            get => (bool)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }
        #endregion

        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel),
                                        typeof(FindWindowEditorViewModel),
                                        typeof(FindWindowEditorView),
                                        new PropertyMetadata(null));

        public FindWindowEditorViewModel ViewModel
        {
            get => (FindWindowEditorViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (FindWindowEditorViewModel)value;
        }
        #endregion

        HighlightBorder hightlightBorder;

        //public IntPtr Value { get; set; }
        public FindWindowEditorView()
        {
            InitializeComponent();
            Mode = true;

            this.WhenActivated(d =>
            {
                this.Bind(ViewModel, vm => vm.TargetWindowTitle, v => v.targetWindowTitle.Text);
                this.Bind(ViewModel, vm => vm.TargetWindowClass, v => v.targetWindowClass.Text);

                this.OneWayBind(ViewModel, vm => vm.Value, v => v.targetWindowTitle.Text,
                          (hWnd) =>    // VM -> View
                          {
                              if (hWnd != IntPtr.Zero)
                              {
                                  DisplayWindowInfo(hWnd);
                                  return User32.GetWindowText(hWnd);
                              }
                              else
                              {
                                  return "";
                              }
                          });


                this.OneWayBind(ViewModel, vm => vm.Value, v => v.targetWindowClass.Text,
                          (hWnd) =>    // VM -> View
                          {
                              if (hWnd != IntPtr.Zero)
                              {
                                  DisplayWindowInfo(hWnd);
                                  StringBuilder ClassName = new StringBuilder(256);
                                  int ret = GetClassName(hWnd, ClassName, ClassName.Capacity);
                                  return ClassName.ToString();
                              }
                              else
                              {
                                  return "";
                              }
                          });

            });
        }

        private static Rectangle RECTtoRectangle(RECT rect) => new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);

        private void ShowInvertRectTracker(IntPtr window)
        {
            if (window != IntPtr.Zero)
            {
                Rectangle WindowRect = GetWindowRect(window);
                if (hightlightBorder.border.Visibility != Visibility.Visible) 
                    hightlightBorder.border.Visibility = Visibility.Visible;

                this.hightlightBorder.Left = WindowRect.Left;
                this.hightlightBorder.Top = WindowRect.Top;
                this.hightlightBorder.Width = WindowRect.Width;
                this.hightlightBorder.Height = WindowRect.Height;
            }
        }

        /// <summary> Show informations about the given window </summary>
		private void DisplayWindowInfo(IntPtr window)
        {
            try
            {
                // Title
                this.targetWindowTitle.Text = User32.GetWindowText(window);
                // Class
                StringBuilder ClassName = new StringBuilder(256);
                int ret = GetClassName(window, ClassName, ClassName.Capacity);
                this.targetWindowClass.Text = ClassName.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static Rectangle GetWindowRect(IntPtr hWnd)
        {
            System.Diagnostics.Debug.Assert(hWnd != IntPtr.Zero);
            if (User32.GetWindowRect(hWnd, out RECT rect) == false)
                return new System.Drawing.Rectangle();//throw new Exception("GetWindowRect failed");
            return RECTtoRectangle(rect);
        }

        private void Image_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
            {
                this.Cursor = new Cursor("Resource/WinAim.cur", true);
                this.Mode = false;
                if (hightlightBorder is not null)
                    hightlightBorder.Close();
                hightlightBorder = new HighlightBorder();
                hightlightBorder.Show();
            }
        }

        private void Image_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Cursor != Cursors.Arrow)
            {
                ShowInvertRectTracker(ViewModel.Value);
                Cursor = Cursors.Arrow;
                this.Mode = true;
                hightlightBorder.Close();
                hightlightBorder = null;
            }
        }

        private void Image_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (Cursor != Cursors.Arrow)
            {
                IntPtr FoundWindow = User32.WindowFromPoint(User32.GetCursorPos());
                while (true)
                {
                    IntPtr parent = User32.GetParent(FoundWindow);
                    if (parent != IntPtr.Zero)
                        FoundWindow = parent;
                    else
                        break;
                }
                // not this application
                if (new WindowInteropHelper(Application.Current.MainWindow).Handle != FoundWindow 
                    && new WindowInteropHelper(hightlightBorder).Handle != FoundWindow)

                {
                    if (FoundWindow != ViewModel.Value)
                    {
                        // clear old window
                        ShowInvertRectTracker(ViewModel.Value);
                        // set new window
                        ViewModel.Value = FoundWindow;
                        // paint new window
                        ShowInvertRectTracker(ViewModel.Value);
                    }
                    this.targetWindowTitle.Text += " "; // Event Raise...
                    DisplayWindowInfo(ViewModel.Value);
                }
            }
        }

        private void Button_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewModel.Value = IntPtr.Zero;
        }
    }
}
