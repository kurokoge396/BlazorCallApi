namespace BlazorApp1.Services
{
    public class ApiCaller
    {
        private readonly IHttpClientFactory _factory;
        private readonly ILogger<ApiCaller> _logger;

        public ApiCaller(IHttpClientFactory factory, ILogger<ApiCaller> logger)
        {
            _factory = factory;
            _logger = logger;
        }

        public async Task<(T? Data, string? Error)> GetAsync<T>(string clientName, string url)
        {
            try
            {
                var client = _factory.CreateClient(clientName);
                var result = await client.GetFromJsonAsync<T>(url);
                return (result, null);
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Timeout when calling {Url}", url);
                return (default, "サーバーからの応答がタイムアウトしました。");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error when calling {Url}", url);
                return (default, $"通信エラーが発生しました: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error when calling {Url}", url);
                return (default, $"予期しないエラーが発生しました: {ex.Message}");
            }
        }
    }
}
