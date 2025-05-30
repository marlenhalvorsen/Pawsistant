using Library.Shared.Model;

namespace PawsistantAPI.Services.Interfaces
{
    public interface IPawsistantService
    {
        Task<ChatMessage> GetResponseAsync(ChatMessage userMessage);
    }
}
