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

namespace SCKK_APP_2023.Services.API
{
    internal class LoginService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly AccountStore _accountStore;

        public LoginService(HttpClient httpClient, string baseUrl, AccountStore accountStore)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl;
            _accountStore = accountStore;
        }

        public async Task LoginAsync(string loginName, string password)
        {
            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException();
            }

            UserLogin userLogin = new UserLogin()
            {
                LoginName = $"{loginName}",
                Password = $"{IterativeHash(password, 300000)}"
            };

            var userLoginJson = JsonSerializer.Serialize(userLogin);
            var requestContent = new StringContent(userLoginJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/api/login", requestContent);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new UserNotFoundException($"Hibás bejelentkezési adatok");
            }
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to login: {response.StatusCode}");
            }
            var token = await response.Content.ReadAsStringAsync();

            _accountStore.CurrentAccount = new Account
            {
                LoginName = userLogin.LoginName,
                Role = null!,
                Token = token,
                Expire = await TokenExpire(token)
            };
        }
        private async Task<DateTime> TokenExpire(string token)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

            return await Task.FromResult(jwtToken.ValidTo);
        }

        private string IterativeHash(string input, int iterations)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                for (int i = 0; i < iterations; i++)
                {
                    bytes = sha256.ComputeHash(bytes);
                }
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
