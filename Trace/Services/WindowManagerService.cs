//https://github.com/microsoft/TemplateStudio?tab=readme-ov-file
//Template Studio
//Copyright (c) .NET Foundation and Contributors.

//All rights reserved.

//MIT License
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the ""Software""), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED AS IS, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Trace.Helpers;
using Trace.Interfaces;
using Trace.Views.Controls;


namespace Trace.Services
{
    public class WindowManagerService : IWindowManagerService
    {
        //private readonly IServiceProvider _serviceProvider;
        private readonly IPageService _pageService;

        public Window MainWindow
            => Application.Current.MainWindow;

        public WindowManagerService(IPageService pageService)
        {
            _pageService = pageService;
        }

        public void OpenInNewWindow(string key, object parameter = null)
        {
            var window = GetWindow(key);
            if (window != null)
            {
                window.Activate();
            }
            else
            {
                window = new BaseWindow();
                var frame = new Frame()
                {
                    Focusable = false,
                    NavigationUIVisibility = NavigationUIVisibility.Hidden
                };

                window.Content = frame;
                var page = _pageService.GetPage(key);
                window.Closed += OnWindowClosed;
                window.Show();
                frame.Navigated += OnNavigated;
                var navigated = frame.Navigate(page, parameter);
            }
        }

        public bool? OpenInDialog(string key, object parameter = null)
        {

            //var shellWindow = new ShellDialogWindow(new ShellDialogViewModel());
            //var frame = shellWindow.GetDialogFrame();
            //frame.Navigated += OnNavigated;
            //shellWindow.Closed += OnWindowClosed;
            //var page = _pageService.GetPage(key);
            //var navigated = frame.Navigate(page, parameter);
            //return shellWindow.ShowDialog(); ;
            return false;
        }

        public Window GetWindow(string key)
        {
            foreach (Window window in Application.Current.Windows)
            {
                var dataContext = window.GetDataContext();
                if (dataContext?.GetType().FullName == key)
                {
                    return window;
                }
            }

            return null;
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            if (sender is Frame frame)
            {
                var dataContext = frame.GetDataContext();
                if (dataContext is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatedTo(e.ExtraData);
                }
            }
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            if (sender is Window window)
            {
                if (window.Content is Frame frame)
                {
                    frame.Navigated -= OnNavigated;
                }

                window.Closed -= OnWindowClosed;
            }
        }


        public void ChangeAllWindowBusy(bool isBusy)
        {
            foreach (Window window in Application.Current.Windows)
            {
                // baseWindowに変換できるか
                if (window is BaseWindow baseWindow)
                {
                    baseWindow.SetIsBusy(isBusy);
                }
            }

        }
    }
}
