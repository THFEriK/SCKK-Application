using System;
using System.Collections.Generic;

namespace SCKK_APP_2023.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string LoginName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateTime LastLogin { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
