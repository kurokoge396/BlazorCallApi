using System.Text.Json.Serialization;

namespace BlazorApp1.Services
{
    public class UserApiService
    {
        private readonly HttpClient _httpClient;

        public UserApiService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("MyApi");
        }

        public async Task<List<User?>> GetUserAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<User>>("api/Users");
        }
    }

    public class User
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; } = null;

        [JsonPropertyName("password")]
        public string password { get; set; } = null;

        [JsonPropertyName("email")]
        public string email { get; set; } = null;
    }
}
