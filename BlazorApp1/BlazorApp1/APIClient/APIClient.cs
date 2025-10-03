using BlazorApp1.Services;

namespace BlazorApp1.APIClient
{
    /// <summary>
    /// インターフェイス
    /// </summary>
    public interface IAPIClient
    {
        public Task<(List<User?> Data, string? ErrorMessage)> GetUsersAsync();
    }

    /// <summary>
    /// APIクライアント
    /// </summary>
    public class APIClient
    {
        private readonly ApiCaller _apiCaller;

        // コンストラクタ
        public APIClient(ApiCaller apiCaller)
        {
            _apiCaller = apiCaller;
        }

        /// <summary>
        /// ユーザ一覧取得
        /// </summary>
        /// <returns>ユーザ一覧</returns>
        public async Task<(List<User?> Data, string? ErrorMessage)> GetUsersAsync()
        {
            return await _apiCaller.GetAsync<List<User>>("MyApi", "api/Users");
        }
    }
}
