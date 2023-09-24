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
    internal class KeyManagerViewModel : ViewModelBase
    {
        private readonly IConfiguration _configuration;
        public KeyManagerViewModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
