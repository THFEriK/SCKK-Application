using SCKK_APP_2023.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCKK_APP_2023.Stores
{
    internal class AccountStore
    {
        public event Action? CurrentAccountChanged;
        private Account _currentAccount = null!;
        public bool IsLoggedIn => CurrentAccount != null;

        public Account CurrentAccount
        {
            get => _currentAccount;
            set
            {
                _currentAccount = value;
                CurrentAccountChanged?.Invoke();
            }
        }

        public void Logout() => CurrentAccount = null!;
    }
}
