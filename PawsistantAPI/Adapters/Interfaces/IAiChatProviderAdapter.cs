using Library.Shared.Model;

namespace PawsistantAPI.Adapters.Interfaces
{
    public interface IAiChatProviderAdapter
    {
        Task<ChatMessage> GetChatMessageAsync(ChatMessage message);   
    }
}
