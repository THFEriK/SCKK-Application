using SCKK_APP_2023.Stores;
using SCKK_APP_2023.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.Configuration;
using SCKK_APP_2023.Services.API;
using System.Configuration;
using SCKK_APP_2023.Services.Navigation;
using System.Windows.Controls;
using System.Windows;
using System.Security;
using SCKK_APP_2023.Views;

namespace SCKK_APP_2023.ViewModels
{
    internal class LoginViewModel : ViewModelBase
    {
        public ICommand LoginCommand { get; }
        public ICommand NavigateSingupCommand { get; }
        private readonly IConfiguration _configuration;

        private string _loginName = String.Empty;
        public string LoginName
        {
            get => _loginName;
            set
            {
                _loginName = value;
                OnPropertyChanged(nameof(LoginName));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private bool _icWorking = false;
        public bool IsWorking
        {
            get => _icWorking;
            set
            {
                _icWorking = value;
                OnPropertyChanged(nameof(IsWorking));
            }
        }
        public MessageViewModel ErrorMessageViewModel { get; }

        public LoginViewModel(AccountStore accountStore, INavigationService navigationService, INavigationService singupNavigationService, IConfiguration configuration)
        {
            ErrorMessageViewModel = new MessageViewModel();
            NavigateSingupCommand = new NavigateCommand(singupNavigationService);
            _configuration = configuration;
            LoginCommand = new LoginCommand(navigationService, this, accountStore);
        }
    }
}
