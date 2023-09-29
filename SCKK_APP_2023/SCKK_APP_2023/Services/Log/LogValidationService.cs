using SCKK_APP_2023.Stores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace SCKK_APP_2023.Services.Log
{
    internal class LogValidationService
    {
        private LogStore? _logStore;
        public LogValidationService(LogStore logstore) 
        { 
            _logStore = logstore;
        }

        public LogValidationService(){ }

        public bool IsValidated(string path, string file, DateTime lastModified)
        {
            // TODO: Hátulról keressen, mert van, hogy két üres sor van a végén nem csak egy
            string[] lines = File.ReadAllLines(System.IO.Path.Combine(path, file));
            string lastLine = lines.Length > 1 ? lines[lines.Length - 1] : string.Empty;
            lines = null;

            const string dateFormat = "yyyy-MM-dd HH:mm:ss";
            DateTime lastDate = DateTime.ParseExact(lastLine.Substring(1, 19), dateFormat, null);

            if ((int)(lastModified - lastDate).TotalSeconds == 0)
            {
                return true;
            }
            return true;
        }
    }
}
