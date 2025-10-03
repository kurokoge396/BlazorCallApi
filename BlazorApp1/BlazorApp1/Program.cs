using BlazorApp1.APIClient;
using BlazorApp1.Components;
using BlazorApp1.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog ���\���i���O�t�@�C���ɕۑ��j
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/app-.log", rollingInterval: RollingInterval.Day) // �����ƂɃt�@�C������
    .CreateLogger();
builder.Host.UseSerilog();

// webapi �p HttpClient ��o�^
builder.Services.AddHttpClient("MyApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7168/"); // �� WebAPI �� URL
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// �ǉ��T�[�r�X
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
