using System.Text.Json.Serialization;

namespace BlazorApp1.Services
{
    public class UserApiService
    {
        private readonly ApiCaller _apiCaller;

        public UserApiService(ApiCaller apiCaller)
        {
            _apiCaller = apiCaller;
        }

        public async Task<(List<User?> Data, string? ErrorMessage)> GetUserAsync()
        {
            return await _apiCaller.GetAsync<List<User>>("MyApi", "api/Users");
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
