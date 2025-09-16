using Library.Shared.Model;
using PawsistantAPI.Adapters.Interfaces;
using PawsistantAPI.Services.Interfaces;

namespace PawsistantAPI.Services
{
    public class PawsistantService : IPawsistantService
    {
        private readonly IAiChatProviderAdapter _IAiChatProviderAdapter;

        public PawsistantService(IAiChatProviderAdapter aiChatProviderAdapter)
        {
            _IAiChatProviderAdapter = aiChatProviderAdapter;
        }
        public async Task<ChatMessage> GetResponseAsync(ChatMessage userMessage)
        {        
            return await _IAiChatProviderAdapter.GetChatMessageAsync(userMessage);
        }
    }
}
