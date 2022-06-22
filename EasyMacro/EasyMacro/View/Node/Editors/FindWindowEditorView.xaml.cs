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
        [DllImport("gdi32.dll")]
        public static extern int SetROP2(IntPtr hdc, int fnDrawMode);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreatePen(int fnPenStyle, int nWidth, uint crColor);
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);
        [DllImport("gdi32.dll")]
        public static extern uint Rectangle(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);
        [DllImport("user32.dll")]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        public enum RopMode : int
        {
            R2_NOT = 6
        }

        public enum PenStyles : int
        {
            PS_INSIDEFRAME = 6
        }

        public enum GetSystem_Metrics : int
        {
            SM_CXBORDER = 5,
            SM_CXFULLSCREEN = 16,
            SM_CYFULLSCREEN = 17
        }

        public enum StockObjects : int
        {
            NULL_BRUSH = 5
        }
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

        public IntPtr LastWindow { get; set; }
        public string _targetWindowTitle = null;
        public string _targetWindowClass = null;
        public FindWindowEditorView()
        {
            InitializeComponent();
            Mode = true;

            this.WhenActivated(d =>
            {
                this.Bind(ViewModel, vm => vm.TargetWindowTitle, v => v.targetWindowTitle.Text,
                                (winTitle) => // VM -> View
                                {
                                    IntPtr hWnd = User32.FindWindow(targetWindowClass.Text, winTitle);
                                    if (hWnd != IntPtr.Zero)
                                    {
                                        LastWindow = hWnd;
                                    }
                                    else
                                    {
                                        // Text로 Window Handle을 찾을 수 없다고 빨간색 잉크 표시
                                    }
                                    if (string.IsNullOrEmpty(winTitle) is true)
                                    {
                                        return _targetWindowTitle ?? "";
                                    }
                                    else
                                    {
                                        _targetWindowTitle = winTitle;
                                        return winTitle;
                                    }
                                    
                                },
                                (winTitle) => winTitle);
                this.Bind(ViewModel, vm => vm.TargetWindowClass, v => v.targetWindowClass.Text,
                                (winClass) => // VM -> View
                                {
                                    IntPtr hWnd = User32.FindWindow(winClass, targetWindowTitle.Text);
                                    if (hWnd != IntPtr.Zero)
                                    {
                                        LastWindow = hWnd;
                                    }
                                    else
                                    {
                                        // Text로 Window Handle을 찾을 수 없다고 빨간색 잉크 표시
                                    }
                                    if (string.IsNullOrEmpty(winClass) is true)
                                    {
                                        return _targetWindowClass ?? "";
                                    }
                                    else
                                    {
                                        _targetWindowClass = winClass;
                                        return winClass;
                                    }
                                },
                                (winClass) => winClass);


                this.Bind(ViewModel, vm => vm.Value, v => v.targetWindowTitle.Text,
                          (value) =>    // VM -> View
                          {
                              if (value != IntPtr.Zero)
                              {
                                  LastWindow = value;
                                  DisplayWindowInfo(value);
                                  return User32.GetWindowText(value);
                              }
                              else
                              {
                                  return "";
                              }
                          },
                          (v) =>        // View -> VM
                          {
                              return LastWindow;
                          });

                
                this.Bind(ViewModel, vm => vm.Value, v => v.targetWindowClass.Text,
                          (value) =>    // VM -> View
                          {
                              if (value != IntPtr.Zero)
                              {
                                  LastWindow = value;
                                  DisplayWindowInfo(value);
                                  return User32.GetWindowText(value);
                              }
                              else
                              {
                                  return "";
                              }
                          },
                          (v) =>        // View -> VM
                          {
                              return LastWindow;
                          });
                
            });
        }

        private static Rectangle RECTtoRectangle(RECT rect) => new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);

        private static void ShowInvertRectTracker(IntPtr window)
        {
            if (window != IntPtr.Zero)
            {
                // get the coordinates from the window on the screen
                Rectangle WindowRect = GetWindowRect(window);
                // get the window's device context
                IntPtr dc = GetWindowDC(window);

                // Create an inverse pen that is the size of the window border
                SetROP2(dc, (int)RopMode.R2_NOT);

                Color color = Color.FromArgb(0, 255, 0);
                IntPtr Pen = CreatePen((int)PenStyles.PS_INSIDEFRAME, 3 * User32.GetSystemMetrics(User32.SystemMetric.SM_CXBORDER), (uint)color.ToArgb());

                // Draw the rectangle around the window
                IntPtr OldPen = SelectObject(dc, Pen);
                IntPtr OldBrush = SelectObject(dc, Gdi32.GetStockObject(Gdi32.StockObject.NULL_BRUSH));
                Rectangle(dc, 0, 0, WindowRect.Width, WindowRect.Height);

                SelectObject(dc, OldBrush);
                SelectObject(dc, OldPen);

                //release the device context, and destroy the pen
                User32.ReleaseDC(window, dc);
                Gdi32.DeleteObject(Pen);
            }
        }

        /// <summary>
		/// return the window from the given point
		/// </summary>
		/// <param name="point"></param>
		/// <returns>if return == IntPtr.Zero no window was found</returns>
		static IntPtr ChildWindowFromPoint(POINT point)
        {
            IntPtr WindowPoint = User32.WindowFromPoint(point);
            if (WindowPoint == IntPtr.Zero)
                return IntPtr.Zero;

            if (User32.ScreenToClient(WindowPoint, ref point) == false)
                throw new Exception("ScreenToClient failed");

            IntPtr IWindow = User32.ChildWindowFromPointEx(WindowPoint, point, 0);
            if (IWindow == IntPtr.Zero)
                return WindowPoint;

            if (User32.ClientToScreen(WindowPoint, ref point) == false)
                throw new Exception("ClientToScreen failed");

            if (User32.IsChild(User32.GetParent(IWindow), IWindow) == false)
                return IWindow;

            // create a list to hold all childs under the point
            ArrayList WindowList = new ArrayList();
            while (IWindow != IntPtr.Zero)
            {
                User32.GetWindowRect(IWindow, out RECT rect);
                if (RECTtoRectangle(rect).Contains(point))
                    WindowList.Add(IWindow);
                IWindow = User32.GetWindow(IWindow, User32.GetWindowCommands.GW_HWNDNEXT);
            }

            // search for the smallest window in the list
            int MinPixel = User32.GetSystemMetrics(User32.SystemMetric.SM_CXFULLSCREEN) * User32.GetSystemMetrics(User32.SystemMetric.SM_CYFULLSCREEN);
            for (int i = 0; i < WindowList.Count; ++i)
            {
                User32.GetWindowRect((IntPtr)WindowList[i], out RECT rect);
                Rectangle drect = RECTtoRectangle(rect);
                int ChildPixel = drect.Width * drect.Height;
                if (ChildPixel < MinPixel)
                {
                    MinPixel = ChildPixel;
                    IWindow = (IntPtr)WindowList[i];
                }
            }
            return IWindow;
        }

        /// <summary>
		/// Show informations about the given window
		/// </summary>
		/// <param name="window"></param>
		private void DisplayWindowInfo(IntPtr window)
        {
            // Caption
            /*
            StringBuilder WindowText = new StringBuilder(ApiWrapper.Window.GetWindowTextLength(window) + 1);
            ApiWrapper.Window.GetWindowText(window, WindowText, WindowText.Capacity);
            textBoxCaption.Text = WindowText.ToString();
            */
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


            // Rect
            // User32.GetWindowRect(window, out RECT rect);
            // Rectangle drect = RECTtoRectangle(rect);
            // ~~ = drect.ToString();

        }

        private static Rectangle GetWindowRect(IntPtr hWnd)
        {
            System.Diagnostics.Debug.Assert(hWnd != IntPtr.Zero);
            if (User32.GetWindowRect(hWnd, out RECT rect) == false)
                throw new Exception("GetWindowRect failed");
            return RECTtoRectangle(rect);
        }

        private void Image_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
            {
                this.Cursor = new Cursor("Resource/WinAim.cur", true);
                this.Mode = false;
            }
        }

        private void Image_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Cursor != Cursors.Arrow)
            {
                ShowInvertRectTracker(LastWindow);
                LastWindow = IntPtr.Zero;
                Cursor = Cursors.Arrow;
                this.Mode = true;
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
                if (new WindowInteropHelper(Application.Current.MainWindow).Handle != FoundWindow)
                {
                    if (FoundWindow != LastWindow)
                    {
                        // clear old window
                        ShowInvertRectTracker(LastWindow);
                        // set new window
                        LastWindow = FoundWindow;
                        // paint new window
                        ShowInvertRectTracker(LastWindow);
                    }
                    this.targetWindowTitle.Text += " "; // Event Raise...
                    DisplayWindowInfo(LastWindow);
                }
            }
        }

    }
}
