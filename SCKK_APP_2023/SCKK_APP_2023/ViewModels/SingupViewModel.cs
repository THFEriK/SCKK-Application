using Microsoft.Extensions.Configuration;
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
    class SingupViewModel : ViewModelBase
    {
        public ICommand SingupCommand { get; }
        private readonly IConfiguration _configuration;

        private string _loginName = String.Empty;
        private string _password = String.Empty;
        private string _passwordConfirm = String.Empty;
        private string _token = String.Empty;

        public string LoginName
        {
            get => _loginName;
            set
            {
                _loginName = value;
                OnPropertyChanged(nameof(LoginName));
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string PasswordConfirm
        {
            get => _passwordConfirm;
            set
            {
                _passwordConfirm = value;
                OnPropertyChanged(nameof(PasswordConfirm));
            }
        }
        public string Token
        {
            get => _token;
            set
            {
                _token = value;
                OnPropertyChanged(nameof(Token));
            }
        }

        public SingupViewModel(AccountStore accountStore, INavigationService navigationService, IConfiguration configuration)
        {
            _configuration = configuration;
            SingupCommand = new SingupCommand(navigationService, this, accountStore);
        }
    }
}
