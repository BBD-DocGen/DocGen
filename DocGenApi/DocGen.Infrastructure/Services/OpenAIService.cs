using System.Text;
using System.Text.Json;
using DocGen.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DocGen.Infrastructure.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenAIService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenAI:ApiKey"];
            Console.WriteLine($"API Key (for debugging, remove this in production): {_apiKey}");
        }

        public async Task<string> GenerateSummaryAsync(string fileName, string fileContents)
        {
            var messages = new[]
            {
                new 
                {
                    role = "system",
                    content = "You are an assistant skilled in summarizing technical documents with clarity and precision."
                },
                new 
                {
                    role = "user",
                    content = $"Please summarize the following document named '{fileName}':\n\n{fileContents}"
                }
            };

            var requestBody = new
            {
                model = "gpt-3.5-turbo", 
                messages = messages,
            };

            var serializedRequestBody = JsonSerializer.Serialize(requestBody);
            Console.WriteLine($"Request Body: {serializedRequestBody}");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.PostAsync(
                "https://api.openai.com/v1/chat/completions",
                new StringContent(serializedRequestBody, Encoding.UTF8, "application/json"));
            
            //TODO: Change this to use deserialization instead

            var responseContent = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseContent);
            var root = doc.RootElement;
            var choices = root.GetProperty("choices");
            var firstChoice = choices[0];
            var message = firstChoice.GetProperty("message");
            var content = message.GetProperty("content").GetString();

            Console.WriteLine($"Extracted Summary: {content}");

            return content ?? "Summary could not be generated.";
        }
    }
    
}
