using Microsoft.Extensions.Configuration;
using SCKK_App.Commands;
using SCKK_APP_2023.Models;
using SCKK_APP_2023.Services.Log;
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
    internal class LogFilterViewModel : ViewModelBase
    {
        private readonly NavigationBarViewModel NavigationBarViewModel;
        private readonly LogStore _logStore;
        private readonly IConfiguration _configuration;
        public ICommand UploadCommand { get; }

        private List<LogCallModel> _logCallModels;
        public List<LogCallModel> LogCallModels
        {
            get { return _logCallModels; }
            set { 
                _logCallModels = value;
                OnPropertyChanged(nameof(LogCallModels));
            }
        }

        public LogFilterViewModel(AccountStore accountStore, LogStore logStore, INavigationService navigationService, IConfiguration configuration)
        {
            _logStore = logStore;
            var LogFilterService = new LogFilterService(logStore);
            LogFilterService.Filter();
            _logCallModels = _logStore.CurrentLog.Calls;
            _configuration = configuration;
            UploadCommand = new UploadCommand(navigationService,logStore,this,accountStore);
        }
    }
}
