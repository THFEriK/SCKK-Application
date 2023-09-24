using System;
using System.Collections.Generic;

namespace SCKK_APP_2023.Models
{
    public partial class RegistrationToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = null!;
        public DateTime Created { get; set; }
    }
}
