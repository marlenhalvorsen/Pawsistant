using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pawsistant.Services.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;

        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("jwt");
            var identity = string.IsNullOrWhiteSpace(token)
                ? new ClaimsIdentity()
                : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }
        public void NotifyUserAuthentication(string jwtToken)
        {
            var identity = new ClaimsIdentity(ParseClaimsFromJwt(jwtToken), "jwt");
            var user = new ClaimsPrincipal(identity);

            // Create an AuthenticationState object to represent the authenticated user
            var authenticationState = new AuthenticationState(user);

            // Notify that the authentication state has changed (synchronously)
            NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
        }
        public void NotifyUserLogout()
        {
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            var authenticationState = new AuthenticationState(anonymous);

            // Notify that the authentication state has changed (synchronously)
            NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = WebEncoders.Base64UrlDecode(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }
        public bool IsTokenExpired(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                return true; // Token is invalid
            }

            var exp = jwtToken?.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
            if (exp == null)
            {
                return true; // No expiration claim found
            }

            var expirationTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp)).UtcDateTime;
            return expirationTime < DateTime.UtcNow;
        }
    }
}
