using SCKK_APP_2023.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCKK_APP_2023.Stores
{
    internal class LogStore
    {
        public event Action? CurrentLogChanged;
        private LogModel _currentLog = new LogModel();

        public LogModel CurrentLog
        {
            get => _currentLog;
            set
            {
                _currentLog = value;
                CurrentLogChanged?.Invoke();
            }
        }
    }
}
