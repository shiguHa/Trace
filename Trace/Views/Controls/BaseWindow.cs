using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

namespace Trace.Views.Controls
{
    public class BaseWindow : Window
    {
        private BusyShield _busyShieldControl;
        private Button _closeButton;

        #region TitlebarBackground
        public Brush TitleBarBackground
        {
            get { return (Brush)GetValue(TitleBarBackgroundProperty); }
            set { SetValue(TitleBarBackgroundProperty, value); }
        }

        public static readonly DependencyProperty TitleBarBackgroundProperty = DependencyProperty.Register(
            nameof(TitleBarBackgroundProperty), typeof(Brush), typeof(BaseWindow), new PropertyMetadata(new SolidColorBrush(Colors.White)));
        #endregion

        #region TitlebarForeground
        public Brush TitleBarForeground
        {
            get { return (Brush)GetValue(TitleBarForegroundProperty); }
            set { SetValue(TitleBarForegroundProperty, value); }
        }

        public static readonly DependencyProperty TitleBarForegroundProperty = DependencyProperty.Register(
            nameof(TitleBarForegroundProperty), typeof(Brush), typeof(BaseWindow), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
        #endregion




        static BaseWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseWindow), new FrameworkPropertyMetadata(typeof(BaseWindow)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var MinimizeButton = GetTemplateChild("MinimizeButton") as Button;
            var MaximizeButton = GetTemplateChild("MaximizeButton") as Button;
            _closeButton = GetTemplateChild("CloseButton") as Button;
            _busyShieldControl = GetTemplateChild("BusyShieldControl") as BusyShield;

            MinimizeButton.Click += MinimizeButton_Click;
            MaximizeButton.Click += MaximizeButton_Click;
            _closeButton.Click += CloseButton_Click;

            SourceInitialized += Window_SourceInitialized;
        }

        public void SetIsBusy(bool isBusy)
        {
            _busyShieldControl.IsBusy = isBusy;
            _closeButton.IsEnabled = !isBusy;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            switch (this.WindowState)
            {
                case WindowState.Normal:
                    this.WindowState = WindowState.Maximized;
                    break;

                case WindowState.Maximized:
                    this.WindowState = WindowState.Normal;
                    break;
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }



        #region WindwoChrome使用時に最大化する問題回避

        //https://ja.stackoverflow.com/questions/56806/windowchrome%E3%82%92%E9%81%A9%E7%94%A8%E3%81%97%E3%81%9F%E3%82%A6%E3%82%A3%E3%83%B3%E3%83%89%E3%82%A6%E3%82%92%E6%9C%80%E5%A4%A7%E5%8C%96%E3%81%97%E3%81%9F%E3%81%A8%E3%81%8D%E3%81%AB%E7%94%BB%E9%9D%A2%E3%82%B5%E3%82%A4%E3%82%BA%E3%81%B4%E3%81%A3%E3%81%9F%E3%82%8A%E3%81%A7%E6%9C%80%E5%A4%A7%E5%8C%96%E3%81%95%E3%81%9B%E3%81%9F%E3%81%84
        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            IntPtr handle = (new WindowInteropHelper(this)).Handle;
            HwndSource.FromHwnd(handle).AddHook(WinProc);
        }

        private IntPtr WinProc(
          IntPtr hwnd,
          int message,
          IntPtr wparam,
          IntPtr lparam,
          ref bool handled)
        {
            switch (message)
            {
                case 0x0024: /* WM_GETMINMAXINFO */
                    handled = WmGetMinMaxInfo(hwnd, lparam, this);
                    break;
            }

            return IntPtr.Zero;
        }

        private static bool WmGetMinMaxInfo(IntPtr hwnd, IntPtr lparam, Window window)
        {
            NativeMethods.MinMaxInformation mmi = (NativeMethods.MinMaxInformation)Marshal.PtrToStructure(lparam, typeof(NativeMethods.MinMaxInformation));

            IntPtr monitor = NativeMethods.MonitorFromWindow(hwnd, 2);
            if (monitor == IntPtr.Zero)
            {
                return false;
            }

            NativeMethods.MonitorInformation monitorInformation = new NativeMethods.MonitorInformation();
            if (!NativeMethods.GetMonitorInfo(monitor, monitorInformation))
            {
                return false;
            }

            NativeMethods.Rectangle workArea = monitorInformation.rcWork;
            NativeMethods.Rectangle monitorArea = monitorInformation.rcMonitor;
            mmi.ptMaxPosition.X = Math.Abs(workArea.Left - monitorArea.Left);
            mmi.ptMaxPosition.Y = Math.Abs(workArea.Top - monitorArea.Top);
            mmi.ptMaxSize.X = Math.Abs(workArea.Right - workArea.Left);
            mmi.ptMaxSize.Y = Math.Abs(workArea.Bottom - workArea.Top);

            Point magnification = GetDeviceToLogicalCoefficient(window);

            if (!double.IsInfinity(window.MinWidth) && !double.IsNaN(window.MinWidth))
            {
                mmi.ptMinTrackSize.X = (int)(window.MinWidth * magnification.X);
            }

            if (!double.IsInfinity(window.MinHeight) && !double.IsNaN(window.MinHeight))
            {
                mmi.ptMinTrackSize.Y = (int)(window.MinHeight * magnification.Y);
            }

            if (!double.IsInfinity(window.MaxWidth) && !double.IsNaN(window.MaxWidth))
            {
                mmi.ptMaxTrackSize.X = (int)(window.MaxWidth * magnification.X);
            }

            if (!double.IsInfinity(window.MaxHeight) && !double.IsNaN(window.MaxHeight))
            {
                mmi.ptMaxTrackSize.Y = (int)(window.MaxHeight * magnification.Y);
            }

            Marshal.StructureToPtr(mmi, lparam, true);
            return true;
        }

        internal static Point GetDeviceToLogicalCoefficient(Window window)
        {
            PresentationSource presentationSource = PresentationSource.FromVisual(window);
            if (presentationSource == null || presentationSource.CompositionTarget == null)
            {
                return new Point(1.0, 1.0);
            }

            return new Point
            {
                X = presentationSource.CompositionTarget.TransformToDevice.M11,
                Y = presentationSource.CompositionTarget.TransformToDevice.M22
            };
        }

        #endregion WindwoChrome使用時に最大化する問題回避
    }

    internal static class NativeMethods
    {
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MonitorInformation lpmi);

        [StructLayout(LayoutKind.Sequential)]
        internal struct NativePoint
        {
            internal int X;
            internal int Y;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        internal struct MinMaxInformation
        {
            internal NativePoint ptReserved;
            internal NativePoint ptMaxSize;
            internal NativePoint ptMaxPosition;
            internal NativePoint ptMinTrackSize;
            internal NativePoint ptMaxTrackSize;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        internal struct Rectangle
        {
            internal int Left;
            internal int Top;
            internal int Right;
            internal int Bottom;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
        internal class MonitorInformation
        {
            internal int cbSize = Marshal.SizeOf(typeof(MonitorInformation));
            internal Rectangle rcMonitor = new Rectangle();
            internal Rectangle rcWork = new Rectangle();
            internal int dwFlags = 0;
        }
    }
}