using Microsoft.Win32;
using SCKK_APP_2023.Services.API;
using SCKK_APP_2023.Services.Navigation;
using SCKK_APP_2023.Stores;
using SCKK_APP_2023.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using SCKK_APP_2023.Models;
using SCKK_APP_2023.Services.Log;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace SCKK_APP_2023.Commands
{
    internal class FileDialogCommand : CommandBase
    {
        private readonly LogFileManagerViewModel _viewModel;
        private readonly INavigationService _navigationService;
        private readonly AccountStore _accountStore;
        private readonly LogValidationService _logValidationService = new LogValidationService();

        public FileDialogCommand(INavigationService navigationService, LogFileManagerViewModel viewModel, AccountStore accountStore)
        {
            _viewModel = viewModel;
            _accountStore = accountStore;
            _navigationService = navigationService;
        }

        //TODO: Nagyrészét átvinni Service-be és függvényekre szellni
        public async override void Execute(object? parameter)
        {
            string selectedFolderPath = String.Empty;
            ObservableCollection<LogFileModel> logFiles = new ObservableCollection<LogFileModel>();

            await Task.Run(() =>
            {
                try
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();

                    openFileDialog.Title = "SeeMTA Logs Mappa";
                    openFileDialog.Filter = "Mappa|*.folder";
                    openFileDialog.CheckFileExists = false;
                    openFileDialog.CheckPathExists = true;
                    openFileDialog.FileName = "Válassz mappát";

                    // Ha a felhasználó kiválasztotta a mappát, akkor jelenítsd meg a kiválasztott mappa nevét
                    if (openFileDialog.ShowDialog() == true)
                    {
                        selectedFolderPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName)!;
                    }

                    string[] excludedFileNames = new string[] { "CEGUI.log", "clientscript.log", "console-input.log", "mods.log" };
                    foreach (string file in Directory.GetFiles(selectedFolderPath, "*.log", SearchOption.TopDirectoryOnly)
                                                        .Where(file => !excludedFileNames.Any(excludedFile => file.EndsWith(excludedFile)))
                                                        .ToArray())
                    {
                        FileInfo fileInfo = new FileInfo(file);

                        // Az .log fájl neve, módosítási dátuma és mérete
                        string name = fileInfo.Name;
                        DateTime lastModified = fileInfo.LastWriteTime;
                        long size = fileInfo.Length;

                        // Az adatok hozzáadása a listához
                        logFiles.Add(new LogFileModel() { Name = name, LastModified = lastModified, Size = size, IsValidated = (_logValidationService.IsValidated(selectedFolderPath, file, lastModified)) });
                    }

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            });
            _viewModel.FilePath = selectedFolderPath;
            _viewModel.LogFiles = logFiles;
        }

        //TODO
    }
}
