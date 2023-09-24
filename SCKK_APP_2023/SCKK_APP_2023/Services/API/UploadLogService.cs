using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SCKK_APP_2023.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using SCKK_APP_2023.Stores;
using System.Net;
using SCKK_APP_2023.Exceptions;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Linq;

namespace SCKK_APP_2023.Services.API
{
    internal class UploadLogService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly AccountStore _accountStore;

        public UploadLogService(HttpClient httpClient, string baseUrl, AccountStore accountStore)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl;
            _accountStore = accountStore;
        }

        public async Task UploadAsync(List<LogCallModel> logCallModel)
        {
            if (logCallModel == null)
            {
                throw new ArgumentNullException();
            }

            var logCallModelJson = JsonSerializer.Serialize(logCallModel);
            var requestContent = new StringContent(logCallModelJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/api/log", requestContent);
            
            List<int> ints = new List<int>() { 1,2,34,5,6 };
        }
    }
}
