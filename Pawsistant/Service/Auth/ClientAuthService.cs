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
        private readonly ILocalStorageService _localStorage;
        private readonly CustomAuthStateProvider _authStateProvider;

        public string JwtToken { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;

        public ClientAuthService(HttpClient http, ILocalStorageService localStorage, CustomAuthStateProvider authStateProvider)
        {
            _http = http;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> LoginAsync(LoginDTO dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", dto);
            if (!response.IsSuccessStatusCode) return false;

            var result = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
            JwtToken = result?.Token ?? string.Empty;
            Email = result?.Email ?? string.Empty;

            await _localStorage.SetItemAsync("jwt", JwtToken);
            await _localStorage.SetItemAsync("email", Email);

            _authStateProvider.NotifyUserAuthentication(JwtToken);
            return true;
        }

        public async Task<bool> RegisterAsync(RegisterDTO dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<HttpRequestMessage?> CreateAuthorizedRequest(HttpMethod method, string url)
        {
            var jwt = await _localStorage.GetItemAsync<string>("jwt");
            if (string.IsNullOrWhiteSpace(jwt)) return null;

            var request = new HttpRequestMessage(method, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);
            return request;
        }

        public async Task<bool> IsLoggedInAsync()
        {
            var jwt = await _localStorage.GetItemAsync<string>("jwt");
            return !string.IsNullOrWhiteSpace(jwt) && !_authStateProvider.IsTokenExpired(jwt);
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("jwt");
            await _localStorage.RemoveItemAsync("email");
            JwtToken = string.Empty;
            Email = string.Empty;

             _authStateProvider.NotifyUserLogout();
        }
    }
}
