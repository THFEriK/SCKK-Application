using SCKK_APP_2023.Commands;
using SCKK_APP_2023.Services.API;
using SCKK_APP_2023.Services.Navigation;
using SCKK_APP_2023.Stores;
using SCKK_APP_2023.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SCKK_App.Commands
{
    internal class UploadCommand : CommandBase
    {
        private readonly LogFilterViewModel _viewModel;
        private readonly INavigationService _navigationService;
        private readonly LogStore _logStore;
        private readonly AccountStore _accountStore;
        private UploadLogService _logService;

        public UploadCommand(INavigationService navigationService, LogStore logStore, LogFilterViewModel viewModel, AccountStore accountStore)
        {
            _viewModel = viewModel;
            _accountStore = accountStore;
            _navigationService = navigationService;
            _logStore = logStore;
            _logService = new UploadLogService(new HttpClient(), "https://localhost:7065", accountStore);
        }

        public override async void Execute(object? parameter)
        {
            await _logService.UploadAsync(_logStore.CurrentLog.Statuses);
            await _logService.UploadAsync(_logStore.CurrentLog.Calls);
        }
    }
}
