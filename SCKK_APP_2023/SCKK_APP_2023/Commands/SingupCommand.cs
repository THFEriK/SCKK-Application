using SCKK_APP_2023.Services.API;
using SCKK_APP_2023.Models;
using SCKK_APP_2023.Services.Navigation;
using SCKK_APP_2023.Stores;
using SCKK_APP_2023.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SCKK_APP_2023.Commands
{
    class SingupCommand : CommandBase
    {
        private readonly SingupViewModel _viewModel;
        private readonly INavigationService _navigationService;
        private readonly AccountStore _accountStore;

        public SingupCommand(INavigationService navigationService, SingupViewModel viewModel, AccountStore accountStore)
        {
            _viewModel = viewModel;
            _accountStore = accountStore;
            _navigationService = navigationService;
        }

        public async override void Execute(object? parameter)
        {
            UserSingup userSingup = new UserSingup()
            {
                LoginName = $"{_viewModel.LoginName}",
                Password = $"{IterativeHash(_viewModel.Password,300000)}",
                Token = $"{_viewModel.Token}"
            };

            //TODO: Az URL-t máshol tárolni és a HttpClient()-et lekezelni
            var singupService = new SingupService(new HttpClient(), "https://localhost:7065");
            var result = await singupService.SingupAsync(userSingup);
            _accountStore.CurrentAccount = result;

            _navigationService.Navigate();
        }

        private string IterativeHash(string input, int iterations)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                for (int i = 0; i < iterations; i++)
                {
                    bytes = sha256.ComputeHash(bytes);
                }
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
