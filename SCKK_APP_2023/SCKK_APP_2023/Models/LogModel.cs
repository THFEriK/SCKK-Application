using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCKK_APP_2023.Models
{
    public class LogModel
    {
        public string LogPath { get; set; }
        public LogFileModel File { get; set; } = new LogFileModel();
        public List<LogCallModel> Calls { get; set; } = new List<LogCallModel>();
    }
}
