using Library.Shared.Auth;

namespace PawsistantAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterUserAsync(RegisterDTO userDTO);
        Task<string> LoginAsync(LoginDTO userDTO);
    }
}
