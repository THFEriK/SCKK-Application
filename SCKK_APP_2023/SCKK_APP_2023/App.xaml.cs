using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SCKK_APP_2023.Services;
using SCKK_APP_2023.Services.Navigation;
using SCKK_APP_2023.Stores;
using SCKK_APP_2023.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SCKK_APP_2023
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;
        private IConfiguration _configuration;

        public App()
        {
            _configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .Build();

            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<AccountStore>();
            services.AddSingleton<NavigationStore>();
            services.AddSingleton<LogStore>();
            services.AddSingleton<INavigationService>(s => CreateHomeNavigationService(s));
            services.AddSingleton<NavigationBarViewModel>(CreateNavigationBarViewModel);
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MainWindow>(s => new MainWindow()
            {
                DataContext = s.GetRequiredService<MainViewModel>()
            });

            services.AddTransient<HomeViewModel>(s => new HomeViewModel(s.GetRequiredService<AccountStore>(), _configuration));
            services.AddTransient<LoginViewModel>(s => new LoginViewModel(s.GetRequiredService<AccountStore>(), s.GetRequiredService<INavigationService>(), CreateSingupNavigationService(s), _configuration));
            services.AddTransient<SingupViewModel>(s => new SingupViewModel(s.GetRequiredService<AccountStore>(), s.GetRequiredService<INavigationService>(), _configuration));
            services.AddTransient<LogFileManagerViewModel>(s => new LogFileManagerViewModel(s.GetRequiredService<AccountStore>(), s.GetRequiredService<LogStore>(), s.GetRequiredService<INavigationService>(), CreateLogFilterNavigationService(s), _configuration));
            services.AddTransient<LogFilterViewModel>(s => new LogFilterViewModel(s.GetRequiredService<AccountStore>(), s.GetRequiredService<LogStore>(), s.GetRequiredService<INavigationService>(), _configuration));
            services.AddTransient<FileManagerViewModel>(s => new FileManagerViewModel(_configuration));
            services.AddTransient<KeyManagerViewModel>(s => new KeyManagerViewModel(_configuration));

            _serviceProvider = services.BuildServiceProvider();
        }

        private NavigationBarViewModel CreateNavigationBarViewModel(IServiceProvider serviceProvider)
        {
            return new NavigationBarViewModel(
                serviceProvider.GetRequiredService<AccountStore>(),
                CreateHomeNavigationService(serviceProvider),
                CreateLogFileManagerNavigationService(serviceProvider),
                CreateLogFilterNavigationService(serviceProvider),
                CreateFileManagerNavigationService(serviceProvider),
                CreateKeyManagerNavigationService(serviceProvider),
                CreateLoginNavigationService(serviceProvider),
                CreateSingupNavigationService(serviceProvider)
                );
        }

        private INavigationService CreateHomeNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<HomeViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>() ,
                () => serviceProvider.GetRequiredService<HomeViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>());
        }
        private INavigationService CreateLogFileManagerNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<LogFileManagerViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<LogFileManagerViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>());
        }

        private INavigationService CreateFileManagerNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<FileManagerViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<FileManagerViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>());
        }

        private INavigationService CreateLogFilterNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<LogFilterViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<LogFilterViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>());
        }




        private INavigationService CreateKeyManagerNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<KeyManagerViewModel>(serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<KeyManagerViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>());
        }
        private INavigationService CreateLoginNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<LoginViewModel>(serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<LoginViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>());
        }
        private INavigationService CreateSingupNavigationService(IServiceProvider serviceProvider)
        {
            return new LayoutNavigationService<SingupViewModel>(serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<SingupViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var initialNavigationService = _serviceProvider.GetRequiredService<INavigationService>();
            initialNavigationService.Navigate();

            MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
