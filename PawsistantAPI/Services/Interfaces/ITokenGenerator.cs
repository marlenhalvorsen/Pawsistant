namespace PawsistantAPI.Services.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateToken(string email, IList<string> roles);
    }
}
