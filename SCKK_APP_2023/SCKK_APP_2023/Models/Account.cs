using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCKK_APP_2023.Models
{
    public class Account
    {
        public string LoginName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime Expire { get; set; }
        public DateTime Start { get; set; }
    }
}
