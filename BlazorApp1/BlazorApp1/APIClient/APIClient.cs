using BlazorApp1.Services;
using System.Text;

namespace BlazorApp1.APIClient
{
    /// <summary>
    /// インターフェイス
    /// </summary>
    public interface IAPIClient
    {
        public Task<bool> Login(string userName, string password);
        public Task<(List<User?> Data, string? ErrorMessage)> GetUsersAsync();
    }

    /// <summary>
    /// APIクライアント
    /// </summary>
    public class APIClient : IAPIClient
    {
        private readonly ApiCaller _apiCaller;

        private string Token = string.Empty;

        // コンストラクタ
        public APIClient(ApiCaller apiCaller)
        {
            _apiCaller = apiCaller;
        }

        /// <summary>
        /// ログイン
        /// </summary>
        /// <param name="userName">ユーザ一名</param>
        /// <param name="password">パスワード</param>
        /// <returns>正否</returns>
        public async Task<bool> Login(string userName, string password)
        {
            var headers = new Dictionary<string, string>
            {
                { "username", userName },
                { "password", password },
                };
            // 送信するデータ
            var data = new { Username = "test", Password = "password" };
            // JSON 変換
            var json = System.Text.Json.JsonSerializer.Serialize(data);
            // HttpContent に変換
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            //var httpContent = new FormUrlEncodedContent(headers);
            var response = await _apiCaller.PostAsync("MyApi", "api/login", content);
            using var doc = System.Text.Json.JsonDocument.Parse(response);
            Token = doc.RootElement.GetProperty("token").GetString() ?? string.Empty;

            return Token != string.Empty;
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
