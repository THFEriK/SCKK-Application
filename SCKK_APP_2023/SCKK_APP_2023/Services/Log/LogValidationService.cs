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
            string[] lines = File.ReadAllLines(System.IO.Path.Combine(path, file));

            for (int i = lines.Length - 1; i >= 0; i--)
            {
                string line = lines[i].Trim();
                if (!string.IsNullOrEmpty(line))
                {
                    const string dateFormat = "yyyy-MM-dd HH:mm:ss";
                    DateTime lastDate = DateTime.ParseExact(line.Substring(1, 19), dateFormat, null);

                    if ((int)(lastModified - lastDate).TotalSeconds == 0)
                    {
                        return true;
                    }
                    break;
                }
            }

            return false;
        }
    }
}
