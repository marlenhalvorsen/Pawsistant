using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Pawsistant.Service.Auth

{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());


        public CustomAuthStateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/auth/me");
                if (response.IsSuccessStatusCode)
                {
                    var identity = new ClaimsIdentity(new[]
                    {
                new Claim(ClaimTypes.Name, "User")
            }, "apiauth");

                    var user = new ClaimsPrincipal(identity);
                    return new AuthenticationState(user);
                }
                else
                {
                    return new AuthenticationState(_anonymous);
                }
            }
            catch
            {
                return new AuthenticationState(_anonymous);
            }
        }
        public void NotifyUserAuthentication()
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "User")
            }, "apiauth");

            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void NotifyUserLogout()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
    }
}
