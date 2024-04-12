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
                messages,
            };

            string serializedRequestBody = JsonSerializer.Serialize(requestBody);
            Console.WriteLine($"Request Body: {serializedRequestBody}");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

            HttpResponseMessage response = await _httpClient.PostAsync(
                "https://api.openai.com/v1/chat/completions",
                new StringContent(serializedRequestBody, Encoding.UTF8, "application/json"));
            
            string responseContent = await response.Content.ReadAsStringAsync();
            JsonDocument doc = JsonDocument.Parse(responseContent);
            JsonElement root = doc.RootElement;
            JsonElement choices = root.GetProperty("choices");
            JsonElement firstChoice = choices[0];
            JsonElement message = firstChoice.GetProperty("message");
            string content = message.GetProperty("content").GetString();

            return content ?? "Uploaded document was saved successfully, but a summary could not be generated.";
        }
    }
    
}
