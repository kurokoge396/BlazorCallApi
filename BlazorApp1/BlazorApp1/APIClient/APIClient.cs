using BlazorApp1.Services;
using BlazorApp1.Shared.Models;
using System.Text;

namespace BlazorApp1.APIClient
{
    /// <summary>
    /// インターフェイス
    /// </summary>
    public interface IAPIClient
    {
        public Task<PostResult> Login(string userName, string password);
        public Task<PostResult> Logout();

        public Task<(List<User?> Data, string? ErrorMessage)> GetUsersAsync();
        public Task<PostResult> CreateUser(User user);
    }

    /// <summary>
    /// APIクライアント
    /// </summary>
    public class APIClient : IAPIClient
    {
        private readonly ApiCaller _apiCaller;

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
        public async Task<PostResult> Login(string userName, string password)
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
            //using var doc = System.Text.Json.JsonDocument.Parse(response.message);

            return new PostResult() { IsSuccess = response.result, Message = response.message };
        }

        /// <summary>
        /// ログアウト
        /// </summary>
        /// <returns>正否</returns>
        public async Task<PostResult> Logout()
        {
            var response = await _apiCaller.PostAsync("MyApi", "api/logout", null);
            //using var doc = System.Text.Json.JsonDocument.Parse(response);

            return new PostResult() { IsSuccess = response.result, Message = response.message };
        }

        /// <summary>
        /// ユーザ一覧取得
        /// </summary>
        /// <returns>ユーザ一覧</returns>
        public async Task<(List<User?> Data, string? ErrorMessage)> GetUsersAsync()
        {
            return await _apiCaller.GetAsync<List<User>>("MyApi", "api/Users");
        }

        /// <summary>
        /// ユーザー作成
        /// </summary>
        /// <param name="user">作成予定のユーザー</param>
        /// <returns>正否</returns>
        public async Task<PostResult> CreateUser(User user)
        {
            // 送信するデータ
            // Dictionary型でデータ作成
            var data = new Dictionary<string, string>
    {
        { "id", user.Id.ToString() },
        { "name", user.Name ?? string.Empty },
        { "password", user.Password ?? string.Empty },
        { "email", user.Email ?? string.Empty }
    };
            // JSON 変換
            var json = System.Text.Json.JsonSerializer.Serialize(data);
            // HttpContent に変換
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _apiCaller.PostAsync("MyApi", "api/Users/Create", content);
            return new PostResult() { IsSuccess = response.result, Message = response.message };
            //using var doc = System.Text.Json.JsonDocument.Parse(response);
            //return doc.RootElement.GetProperty("message").GetString() != string.Empty;
        }
    }
}
