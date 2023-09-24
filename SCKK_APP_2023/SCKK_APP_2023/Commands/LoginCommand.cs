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
using System.Windows;
using System.Net.Sockets;

namespace SCKK_APP_2023.Commands
{
    internal class LoginCommand : CommandBase
    {
        private readonly LoginViewModel _viewModel;
        private readonly INavigationService _navigationService;
        private readonly AccountStore _accountStore;

        public LoginCommand(INavigationService navigationService, LoginViewModel viewModel, AccountStore accountStore)
        {
            _viewModel = viewModel;
            _accountStore = accountStore;
            _navigationService = navigationService;
        }

        public async override void Execute(object? parameter)
        {
            try
            {
                _viewModel.IsWorking = true;
                //TODO: Az URL-t máshol tárolni és a HttpClient()-et lekezelni
                var loginService = new LoginService(new HttpClient(), "https://localhost:7065", _accountStore);
                await loginService.LoginAsync(_viewModel.LoginName, _viewModel.Password);

                _navigationService.Navigate();
            }
            catch (InvalidOperationException)
            {
                _viewModel.ErrorMessageViewModel.Message = "Sikertelen bejelentkezés2";
            }
            catch (TaskCanceledException)
            {
                _viewModel.ErrorMessageViewModel.Message = "A távoli szerver a kérelmet elutasította";
            }
            catch (HttpRequestException)
            {
                _viewModel.ErrorMessageViewModel.Message = "A távoli szerver nem válaszol";
            }
            catch (ArgumentNullException)
            {
                _viewModel.ErrorMessageViewModel.Message = "Minden mezőt kikéne tölteni";
            }
            catch (Exception e)
            {
                _viewModel.ErrorMessageViewModel.Message = "Kezeletlen hiba, jelezd EriK-nek: " + e.Message;
            }
            finally
            {
                _viewModel.IsWorking = false;
            }
        }

        //TODO

    }
}
