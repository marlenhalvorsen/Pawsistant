using Library.Shared.Model;
using PawsistantAPI.Adapters.Interfaces;
using PawsistantAPI.Services.Interfaces;

namespace PawsistantAPI.Services
{
    public class PawsistantService : IPawsistantService
    {
        private readonly IAiChatProviderAdapter _ai;

        public PawsistantService(IAiChatProviderAdapter aiChatProviderAdapter)
        {
            if(aiChatProviderAdapter == null)
                throw new ArgumentNullException(nameof(aiChatProviderAdapter));
            _ai = aiChatProviderAdapter;
        }
        public async Task<ChatMessage> GetResponseAsync(ChatMessage userMessage)
        {
            if(userMessage is null) throw new ArgumentNullException(nameof(userMessage));
            if (string.IsNullOrWhiteSpace(userMessage.Content))
                throw new ArgumentException("Message content must not be empty", nameof(userMessage));

            return await _ai.GetChatMessageAsync(userMessage);
        }
    }
}
