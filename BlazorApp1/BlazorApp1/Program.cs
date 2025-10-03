using BlazorApp1.APIClient;
using BlazorApp1.Components;
using BlazorApp1.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog を構成（ログファイルに保存）
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/app-.log", rollingInterval: RollingInterval.Day) // 日ごとにファイル分割
    .CreateLogger();
builder.Host.UseSerilog();

// webapi 用 HttpClient を登録
builder.Services.AddHttpClient("MyApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7168/"); // ← WebAPI の URL
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 追加サービス
builder.Services.AddScoped<UserApiService>();
builder.Services.AddScoped<ApiCaller>();
builder.Services.AddScoped<IAPIClient, APIClient>();
//builder.Services.AddScoped<APIClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
