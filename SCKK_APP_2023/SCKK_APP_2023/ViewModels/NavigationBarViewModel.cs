using SCKK_APP_2023.Commands;
using SCKK_APP_2023.Services.Navigation;
using SCKK_APP_2023.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SCKK_APP_2023.ViewModels
{
    internal class NavigationBarViewModel : ViewModelBase
    {
        private readonly AccountStore _accountStore;

        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateLogManagerViewCommand { get; }
        public ICommand NavigateFileManagerCommand { get; }
        public ICommand NavigateLogFilterViewCommand { get; }
        public ICommand NavigateKeyManagerCommand { get; }
        public ICommand NavigateLoginCommand { get; }
        public ICommand NavigateSingupCommand { get; }
        public ICommand LogoutCommand { get; }
        public bool IsLoggedIn => _accountStore.IsLoggedIn;
        public bool IsLoggedOut => !_accountStore.IsLoggedIn;

        public NavigationBarViewModel(
            AccountStore accountStore,
            INavigationService homeNavigationService,
            INavigationService logFileManagerNavigationService,
            INavigationService logFilterNavigationService,
            INavigationService filemanagerNavigationService,
            INavigationService keymanagerNavigationService,
            INavigationService loginNavigationService,
            INavigationService singupNavigationService
            )
        {
            _accountStore = accountStore;
            NavigateHomeCommand = new NavigateCommand(homeNavigationService);
            NavigateLogManagerViewCommand = new NavigateCommand(logFileManagerNavigationService);
            NavigateLogFilterViewCommand = new NavigateCommand(logFilterNavigationService);
            NavigateFileManagerCommand = new NavigateCommand(filemanagerNavigationService);
            NavigateKeyManagerCommand = new NavigateCommand(keymanagerNavigationService);
            NavigateLoginCommand = new NavigateCommand(loginNavigationService);
            NavigateSingupCommand = new NavigateCommand(singupNavigationService);
            LogoutCommand = new LogoutCommand(_accountStore);

            _accountStore.CurrentAccountChanged += OnCurrentAccountChanged;
        }

        private void OnCurrentAccountChanged()
        {
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(IsLoggedOut));
        }

        public override void Dispose()
        {
            _accountStore.CurrentAccountChanged -= OnCurrentAccountChanged;

            base.Dispose();
        }
    }
}
