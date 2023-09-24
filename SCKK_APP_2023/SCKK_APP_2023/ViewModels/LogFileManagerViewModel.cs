using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SCKK_APP_2023.Commands;
using SCKK_APP_2023.Models;
using SCKK_APP_2023.Services.Navigation;
using SCKK_APP_2023.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SCKK_APP_2023.ViewModels
{
    internal class LogFileManagerViewModel : ViewModelBase
    {
        public NavigationBarViewModel NavigationBarViewModel { get; }
        private IConfiguration _configuration;
        private LogStore _logModel;
        public ICommand FileDialogCommand { get; }
        public ICommand LogFileManagerCommand { get; }
        public ICommand NavigateLogFilterCommand { get; }

        public string FilePath
        {
            get { return _logModel.CurrentLog.LogPath; }
            set 
            {
                _logModel.CurrentLog.LogPath = value; 
                OnPropertyChanged(nameof(FilePath));
            }
        }
        private ObservableCollection<LogFileModel> _logFiles = new ObservableCollection<LogFileModel>();
        public ObservableCollection<LogFileModel> LogFiles {  
            get { return _logFiles; }  
            set
            {
                _logFiles = value;
                OnPropertyChanged(nameof(LogFiles));
            }
        }



        public LogFileManagerViewModel(AccountStore accountStore, LogStore logStore, INavigationService navigationService, INavigationService logFilterNavigationService, IConfiguration configuration)
        {
            _configuration = configuration;
            _logModel = logStore;
            FileDialogCommand = new FileDialogCommand(navigationService, this, accountStore);
            LogFileManagerCommand = new LogFileManagerCommand(navigationService, logStore, this, accountStore);
            NavigateLogFilterCommand = new NavigateCommand(logFilterNavigationService);
        }
    }
}
