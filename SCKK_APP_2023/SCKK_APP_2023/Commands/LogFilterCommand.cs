using SCKK_APP_2023.Services.Navigation;
using SCKK_APP_2023.Stores;
using SCKK_APP_2023.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCKK_APP_2023.Commands
{
    internal class LogFilterCommand : CommandBase
    {
        private readonly LogFileManagerViewModel _viewModel;
        private readonly INavigationService _navigationService;
        private readonly AccountStore _accountStore;

        public LogFilterCommand(INavigationService navigationService, LogFileManagerViewModel viewModel, AccountStore accountStore)
        {
            _viewModel = viewModel;
            _accountStore = accountStore;
            _navigationService = navigationService;
        }

        public override async void Execute(object? parameter)
        {
            
        }
    }
}
