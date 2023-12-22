using Microsoft.Win32;
using SCKK_APP_2023.Models;
using SCKK_APP_2023.Models.Enums;
using SCKK_APP_2023.Stores;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace SCKK_APP_2023.Services.Log
{
    internal class LogFilterService
    {
        private LogStore _logStore;
        private LogValidationService _logValidationService = new LogValidationService();

        private List<RawLogModel> _taxiLogs = new List<RawLogModel>();
        private List<RawLogModel> _towLogs = new List<RawLogModel>();

        private List<LogStatusModel> _logStatusModels = new List<LogStatusModel>();
        private List<LogCallModel> _logCallModels = new List<LogCallModel>();

        public LogFilterService(LogStore logStore)
        {
            _logStore = logStore;
        }

        public void Filter()
        {
            InitializeLists();
            ReadLogFile();
            FilterCalls();
            FilterAcceptedCalls();
            FilterCancelledCalls();
            //FilterMisses();
            //MergeCallToStatus();
            //FilterEarlyCancelled();
            _logStore.CurrentLog.Calls = _logCallModels.Concat(_logStore.CurrentLog.Calls).ToList();
            _logStore.CurrentLog.Statuses = _logStatusModels.Concat(_logStore.CurrentLog.Statuses).ToList();
        }

        private void InitializeLists()
        {
            _logStore.CurrentLog.Calls = new List<LogCallModel>();
            _logStore.CurrentLog.Statuses = new List<LogStatusModel>();
            _logStatusModels = new List<LogStatusModel>();
            _taxiLogs = new List<RawLogModel>();
            _towLogs = new List<RawLogModel>();
        }

        private void ReadLogFile()
        {
            using (var fs = new FileStream(Path.Combine(_logStore.CurrentLog.LogPath, _logStore.CurrentLog.File.Name), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs, Encoding.Default))
            {
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    FilterType(new RawLogModel() { Lines = line.Split(' '), IsValidated = _logStore.CurrentLog.File.IsValidated });
                }
            }
        }

        private void FilterType(RawLogModel row)
        {
            if (row.Lines.Length > 10)
            {
                if (row.Lines[7] == "(Taxi)]:")
                {
                    _taxiLogs.Add(row);
                }
                if (row.Lines[7].StartsWith("(Autómentő)]:"))
                {
                    _towLogs.Add(row);
                }
            }
        }
        private void FilterCalls()
        {
            foreach (RawLogModel row in _taxiLogs)
            {
                if (row.Lines.Contains("hívás") && row.Lines.Contains("érkezett:"))
                {
                    LogCallModel tmp = new LogCallModel();

                    tmp.CallTime = Time(row.Lines[0], row.Lines[1]);
                    tmp.Identifier = ushort.Parse(row.Lines[11]);
                    tmp.IsValidated = row.IsValidated;

                    FilterDistrictByCallTime(tmp);
                }
            }
        }

        private void FilterDistrictByStatusTime(LogStatusModel call)
        {
            bool isContain = false;
            for (int i = 0; i < _logStatusModels.Count; i++)
            {
                if (_logStatusModels[i].Identifier == call.Identifier && (_logStatusModels[i].StatusTime - call.StatusTime).TotalMinutes < 1)
                {
                    if (_logStatusModels[i].IsValidated == false && call.IsValidated == true)
                    {
                        _logStatusModels[i].Driver = call.Driver;
                        _logStatusModels[i].Status = call.Status;
                        _logStatusModels[i].StatusTime = call.StatusTime;
                        _logStatusModels[i].IsValidated = true;
                    }
                    isContain = true;
                }
            }
            if (!isContain)
                _logStatusModels.Add(call);
        }

        private void FilterDistrictByCallTime(LogCallModel call)
        {
            bool isContain = false;
            for (int i = 0; i < _logCallModels.Count; i++)
            {
                if (_logCallModels[i].Identifier == call.Identifier && (_logCallModels[i].CallTime - call.CallTime).TotalMinutes < 1)
                {
                    if (_logCallModels[i].IsValidated == false && call.IsValidated == true)
                    {
                        _logCallModels[i].CallTime = call.CallTime;
                        _logCallModels[i].IsValidated = true;
                    }
                    isContain = true;
                }
            }
            if (!isContain)
                _logCallModels.Add(call);
        }

        private void FilterAcceptedCalls()
        {
            foreach (RawLogModel row in _taxiLogs)
            {
                int rowLength = row.Lines.Length;

                if (row.Lines.Contains("elfogadta"))
                {
                    LogStatusModel tmp = new LogStatusModel();

                    tmp.Status = CallStatus.Accepted;
                    tmp.StatusTime = Time(row.Lines[0], row.Lines[1]);
                    tmp.Identifier = ushort.Parse(row.Lines[rowLength - 1]);
                    tmp.IsValidated = row.IsValidated;

                    switch (rowLength)
                    {
                        case 16:
                            tmp.Driver = row.Lines[8];
                            break;
                        case 17:
                            tmp.Driver = row.Lines[8] + " " + row.Lines[9];
                            break;
                        case 18:
                            tmp.Driver = row.Lines[8] + " " + row.Lines[9] + " " + row.Lines[10];
                            break;
                        case 19:
                            tmp.Driver = row.Lines[8] + " " + row.Lines[9] + " " + row.Lines[10] + " " + row.Lines[11];
                            break;
                        default:
                            // handle unexpected row
                            break;
                    }
                    /*
                    bool isContain = false;
                    for (int i = 0; i < _logStatusModels.Count; i++)
                    {
                        if (_logStatusModels[i].Identifier == tmp.Identifier
                            && _logStatusModels[i].Status == CallStatus.Unknown
                            && (_logStatusModels[i].StatusTime - tmp.StatusTime).TotalMinutes < 20)
                        {
                            _logStatusModels[i].Status = tmp.Status;
                            _logStatusModels[i].StatusTime = tmp.StatusTime;
                            _logStatusModels[i].Driver = tmp.Driver;
                            _logStatusModels[i].IsValidated = tmp.IsValidated;
                            isContain = true;
                        }
                    }
                    if (!isContain)
                    {
                        FilterDistrictByStatusTime(tmp);
                    }*/

                    FilterDistrictByStatusTime(tmp);
                }
            }
        }

        private void FilterCancelledCalls()
        {
            foreach (RawLogModel row in _taxiLogs)
            {
                int rowLength = row.Lines.Length;

                if (row.Lines.Contains("Törlődött") && row.Lines.Contains("hívás:"))
                {
                    LogStatusModel tmp = new LogStatusModel();

                    tmp.Status = CallStatus.Cancelled;
                    tmp.StatusTime = Time(row.Lines[0], row.Lines[1]);
                    tmp.Identifier = ushort.Parse(row.Lines[12]);
                    tmp.Driver = "Lemondott";
                    tmp.IsValidated = row.IsValidated;

                    bool isContain = false;
                    for (int i = 0; i < _logStatusModels.Count; i++)
                    {
                        if (_logStatusModels[i].Identifier == tmp.Identifier)
                        {
                            /*
                            if (_logStatusModels[i].Status == CallStatus.Unknown && (_logStatusModels[i].StatusTime - tmp.StatusTime).TotalMinutes < 20)
                            {
                                _logStatusModels[i].Status = tmp.Status;
                                _logStatusModels[i].StatusTime = tmp.StatusTime;
                                _logStatusModels[i].Driver = tmp.Driver;
                                _logStatusModels[i].IsValidated = tmp.IsValidated;
                                isContain = true;
                            }
                            else*/
                            if (_logStatusModels[i].Status == CallStatus.Accepted && (_logStatusModels[i].StatusTime - tmp.StatusTime).TotalMinutes < 60)
                            {
                                isContain = true;
                            }
                        }
                    }
                    if (!isContain)
                    {
                        FilterDistrictByStatusTime(tmp);
                    }
                }
            }
        }

        /*
        private void MergeCallToStatus()
        {
            for (int i = 0; i < _logCallModels.Count; i++)
            {
                int j = 0;
                while ((j < _logStatusModels.Count) && ((_logStatusModels[j].Identifier != _logCallModels[i].Identifier) || ((_logStatusModels[j].StatusTime - _logCallModels[i].CallTime).TotalMinutes > 20)))
                {
                    j++;
                }
                if (j < _logStatusModels.Count)
                {
                    _logStatusModels[j].Status = _logCallModels[i].Status;
                    if (_logStatusModels[j].Status == CallStatus.Unknown)
                    {
                        _logStatusModels[j].Driver = _logCallModels[i].Driver;
                        _logStatusModels[j].Status = _logCallModels[i].Status;
                        _logStatusModels[j].IsValidated = _logCallModels[i].IsValidated;
                    }
                    if (!_logCallModels[i].IsValidated)
                    {
                        _logStatusModels[j].IsValidated = _logCallModels[i].IsValidated;
                    }
                }
            }
        }*/

        private DateTime Time(string a, string b) => DateTime.ParseExact(a.TrimStart('[') + b.Remove(b.Length - 1), "yyyy-MM-ddHH:mm:ss", CultureInfo.InvariantCulture);
        /*
        private void FilterEarlyCancelled()
        {
            for (int i = 0; i < _logStatusModels.Count; i++)
            {
                if (_logStatusModels[i].Status == CallStatus.Cancelled && _logStatusModels[i].StatusTime.AddMinutes(1) > _logStatusModels[i].StatusTime)
                {
                    _logStatusModels[i].Status = CallStatus.EarlyCancelled;
                    _logStatusModels[i].Driver = "1 perces";
                }
            }
        }*/
        /*
        private void FilterMisses()
        {
            if (_logStatusModels.Count > 10)
            {
                _logStatusModels = _logStatusModels.OrderBy(x => x.StatusTime).ToList();

                // Hiányzó hívások kiszámolása/megkeresése //
                List<int> missing = new List<int>();
                missing = Enumerable.Range(_logStatusModels.Min(x => x.Identifier), _logStatusModels.Max(x => x.Identifier) - _logStatusModels.Min(x => x.Identifier) - 2).Except(_logStatusModels.Select(x => (int)x.Identifier)).ToList();

                // Hiányzó hívások behelyezése időpont alapján a Hívások közé //
                if (missing.Count < 60)
                {
                    for (int j = 0; j < missing.Count; j++)
                    {
                        int y = 0;
                        while (missing[j] > _logStatusModels[y].Identifier) { y++; };
                        LogStatusModel rm_tmp = new LogStatusModel();
                        rm_tmp.CallTime = _logStatusModels[y].CallTime;
                        rm_tmp.StatusTime = _logStatusModels[y].StatusTime;
                        rm_tmp.Driver = "Hiányzó";
                        rm_tmp.IsValidated = false;
                        rm_tmp.Status = CallStatus.Unknown;
                        rm_tmp.Identifier = (ushort)missing[j];
                        _logStatusModels.Insert(y, rm_tmp);
                    }
                }
            }
        }*/
    }
}