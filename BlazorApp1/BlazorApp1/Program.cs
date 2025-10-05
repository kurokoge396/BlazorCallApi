using BlazorApp1.APIClient;
using BlazorApp1.Components;
using BlazorApp1.Providers;
using BlazorApp1.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "https://localhost:7168",
        ValidAudience = "https://localhost:7168",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("2112121212131435436546547687567867645643523423432564365754757565436643523454324324324343435454354364576346576346grfertv34vt4g54t43f435f4cdrhy53b6hs456s46y5y5465467565463456w46wrd32535g54j6547j477sj65jn64354jsj4e4fyt56j4k64s564h5uyk56sh46s4s64g34g5h4f45f36"))
    };

    // Cookieからトークンを読み取る設定
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.TryGetValue("AuthToken", out var token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// 認証状態プロバイダーを登録
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<CustomAuthenticationService>();

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
