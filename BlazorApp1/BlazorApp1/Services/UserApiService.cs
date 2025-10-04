using BlazorApp1.Shared.Models;

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
}
