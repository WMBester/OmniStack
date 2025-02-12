using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WMB.Api.Models;

namespace WMB.Api.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly HttpClient _httpClient;

        public OpenAIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetResponseAsync(string apiKey, string prompt)
        {
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
            new { role = "system", content = "You are a helpful assistant. Please respond in a cocky manner and always use sarcasm." },
            new { role = "user", content = prompt }
        },
                max_tokens = 100
            };

            var jsonRequestBody = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            int maxRetries = 3;
            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else if ((int)response.StatusCode == 429) // Too Many Requests
                {
                    var retryAfter = response.Headers.RetryAfter?.Delta ?? TimeSpan.FromSeconds(5);
                    await Task.Delay(retryAfter);
                }
                else
                {
                    throw new Exception($"OpenAI API call failed: {response.ReasonPhrase}");
                }
            }

            throw new Exception("OpenAI API call failed after multiple retries.");
        }
    }
}
