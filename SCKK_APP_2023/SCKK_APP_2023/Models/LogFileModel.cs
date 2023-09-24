using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCKK_APP_2023.Models
{
    public class LogFileModel
    {
        public string Name { get; set; } = String.Empty;
        public DateTime LastModified { get; set; }
        public long Size { get; set; }
        public bool IsValidated { get; set; } = false;
    }
}
