using Library.Shared.Auth;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pawsistant.Service.Auth

{
    public interface IClientAuthService
    {
        string JwtToken { get; }
        string Email { get; }

        Task<bool> LoginAsync(LoginDTO dto);
        Task<bool> RegisterAsync(RegisterDTO dto);
        Task<HttpRequestMessage?> CreateAuthorizedRequest(HttpMethod method, string url);
        Task<bool> IsLoggedInAsync();
        Task LogoutAsync();
    }
}
