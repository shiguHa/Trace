using System.IO;
using System.Reflection;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Trace.Interfaces;
using Trace.Services;
using Trace.ViewModels;
using Trace.Views;
using Trace.Views.Controls;

namespace Trace
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private IHost _host;
        public T GetService<T>()
                where T : class
                => _host.Services.GetService(typeof(T)) as T;

        public App()
        {
            var font = new System.Windows.Media.FontFamily("Meiryo");
            var style = new Style(typeof(Window));
            style.Setters.Add(new Setter(Window.FontFamilyProperty, font));
            FrameworkElement.StyleProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(style));
        }


        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            // For more information about .NET generic host see  https://docs.microsoft.com/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.0
            _host = Host.CreateDefaultBuilder(e.Args)
                    .ConfigureAppConfiguration(c =>
                    {
                        c.SetBasePath(appLocation);
                    })
                    .ConfigureServices(ConfigureServices)
                    .Build();

            await _host.StartAsync();

            var window = GetService<MainWindow>();
            var navigationService = GetService<INavigationService>();
            navigationService.Initialize(window.GetNavigationFrame());
            navigationService.NavigateTo(typeof(HomeVM).FullName);
            window.ShowWindow();
        }


        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, Services.NavigationService>();
            services.AddSingleton<IWindowManagerService, WindowManagerService>();
            //services.AddSingleton<IPersistAndRestoreService, PersistAndRestoreService>();
            services.AddTransient<IMessageBoxService, MessageBoxService>();
            services.AddTransient<IFileDialogService, FileDialogService>();

            services.AddTransient<MainWindowVM>();
            services.AddTransient<MainWindow>();

            services.AddTransient<HomeVM>();
            services.AddTransient<HomePage>();

            services.AddTransient<Idea1VM>();
            services.AddTransient<Idea1Page >();

        }



        private void Application_Exit(object sender, ExitEventArgs e)
        {

            //var persistAndRestoreService = GetService<IPersistAndRestoreService>();
            //persistAndRestoreService.PersistData();
        }



    }

}
