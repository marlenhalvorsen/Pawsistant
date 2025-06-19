using Library.Shared.Auth;
using System.Net.Http.Json;
using Blazored.LocalStorage;

using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components.Authorization;

namespace Pawsistant.Service.Auth

{
    public class ClientAuthService : IClientAuthService
    {
        private readonly HttpClient _http;
        private readonly CustomAuthStateProvider _authStateProvider;

        public string JwtToken { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;

        public ClientAuthService(HttpClient http, CustomAuthStateProvider authStateProvider)
        {
            _http = http;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> LoginAsync(LoginDTO dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", dto);
            if (!response.IsSuccessStatusCode) return false;

            _authStateProvider.NotifyUserAuthentication(JwtToken);
            return true;
        }

        public async Task<bool> RegisterAsync(RegisterDTO dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", dto);
            return response.IsSuccessStatusCode;
        }

        

        public async Task<bool> IsLoggedInAsync()
        {
            var response = await _http.GetAsync("api/auth/me");
            return response.IsSuccessStatusCode;
        }

        public async Task LogoutAsync()
        {
            var response = await _http.GetAsync("api/auth/logout", null);
             _authStateProvider.NotifyUserLogout();
        }
    }
}
