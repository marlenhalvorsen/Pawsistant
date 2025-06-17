using Library.Shared.Model;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using PawsistantAPI.Adapters.Interfaces;

namespace PawsistantAPI.Adapters
{
    public class OpenRouterChatProviderAdapter : IAiChatProviderAdapter
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public OpenRouterChatProviderAdapter(IConfiguration config)
        {
            _apiKey = config["OpenRouter:ApiKey"];

            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                throw new InvalidOperationException("OpenRouter API key is missing or empty.");
            }
            Console.WriteLine($"OpenRouter API key loaded: '{_apiKey}'");

            _httpClient = new HttpClient();
        }
        public async Task<ChatMessage> GetChatMessageAsync(ChatMessage message)
        {

            var endpoint = "https://openrouter.ai/api/v1/chat/completions";

            var requestBody = new
            {
                model = "mistralai/mistral-small-3.1-24b-instruct", // <-- This should match what OpenRouter lists as the model ID
                messages = new[]
                {
                    new { role = "system", content = "You are a helpful dog training assistant." },
                    new { role = "user", content = message.Content }
                }
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("HTTP-Referer", "https://github.com/marlenhalvorsen/Pawsistant"); // required
            _httpClient.DefaultRequestHeaders.Add("X-Title", "PawsistantAI"); // custom title shown in OpenRouter dashboard

            try
            {
                var response = await _httpClient.PostAsync(endpoint, jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    return new ChatMessage
                    {
                        Role = "assistant",
                        Content = $"Ai error {response.StatusCode}"
                    };
                }
                var jsonResponse = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(jsonResponse);

                var aiContent = doc
                    .RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                return new ChatMessage
                {
                    Role = "assistant",
                    Content = aiContent ?? "AI responded, but no content found."
                };

            }
            catch (Exception ex)
            {
                return new ChatMessage
                {
                    Role = "assistant",
                    Content = $"Exception: {ex.Message}"
                };
            }
        }
    }
}
