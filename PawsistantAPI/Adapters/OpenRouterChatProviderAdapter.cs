using Library.Shared.Model;
using PawsistantAPI.Adapters.Interfaces;

namespace PawsistantAPI.Adapters
{
    public class OpenRouterChatProviderAdapter : IAiChatProviderAdapter
    {
        private readonly string _apiKey;
        public OpenRouterChatProviderAdapter(IConfiguration config)
        {
            _apiKey = config["OpenRouter:ApiKey"];
        }
        public Task<ChatMessage> GetChatMessageAsync(ChatMessage message)
        {
            
        }
    }
}
