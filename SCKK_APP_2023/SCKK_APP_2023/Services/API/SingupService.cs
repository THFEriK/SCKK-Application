using SCKK_APP_2023.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SCKK_APP_2023.Services.API
{
    class SingupService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public SingupService(HttpClient httpClient, string baseUrl)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl;
        }

        public async Task<Account> SingupAsync(UserSingup userSingup)
        {
            var userSingupJson = JsonSerializer.Serialize(userSingup);
            var requestContent = new StringContent(userSingupJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/api/singup", requestContent);
            if (!response.IsSuccessStatusCode)
            {
                return null!;
                //throw new HttpRequestException($"Failed to login: {response.StatusCode}"); //TODO: Kivétel kezelés
            }
            var token = await response.Content.ReadAsStringAsync();

            return new Account
            {
                LoginName = userSingup.LoginName,
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
    }
}
