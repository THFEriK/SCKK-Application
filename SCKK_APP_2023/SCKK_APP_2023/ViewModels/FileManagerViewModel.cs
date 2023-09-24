using Microsoft.Extensions.Configuration;
using SCKK_APP_2023.Stores;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCKK_APP_2023.ViewModels
{
    internal class FileManagerViewModel : ViewModelBase
    {
        private readonly IConfiguration _configuration;
        public FileManagerViewModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
