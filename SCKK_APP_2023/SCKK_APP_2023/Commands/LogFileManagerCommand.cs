using SCKK_APP_2023.Models;
using SCKK_APP_2023.Services.Log;
using SCKK_APP_2023.Services.Navigation;
using SCKK_APP_2023.Stores;
using SCKK_APP_2023.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SCKK_APP_2023.Commands
{
    internal class LogFileManagerCommand : CommandBase
    {
        private readonly LogFileManagerViewModel _viewModel;
        private readonly INavigationService _navigationService;
        private readonly LogStore _logStore;
        private readonly AccountStore _accountStore;

        public LogFileManagerCommand(INavigationService navigationService, LogStore logStore, LogFileManagerViewModel viewModel, AccountStore accountStore)
        {
            _viewModel = viewModel;
            _accountStore = accountStore;
            _navigationService = navigationService;
            _logStore = logStore;
        }

        public override async void Execute(object? parameter)
        {
            LogFileModel? row = parameter as LogFileModel;
            if (row != null) 
            {
                _logStore.CurrentLog.File = new LogFileModel()
                {
                    IsValidated = row.IsValidated,
                    LastModified = row.LastModified,
                    Size = row.Size,
                    Name = row.Name,
                };
                _viewModel.NavigateLogFilterCommand.Execute(parameter);
            }
        }
    }
}
