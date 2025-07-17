using MyBarMenu.Client.Components;
using MyBarMenu.Client.Services;
using MyBarMenu.Client.Services.Interfaces;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

Env.Load();
var baseUrl = Environment.GetEnvironmentVariable("BACKEND_URL") ?? throw new InvalidOperationException("Backend url not set");

// Registers AccountService as a scoped service. Each time it's injected, a new instance is created with its own HttpClient. The HttpClient is managed by IHttpClientFactory and scoped to the AccountService instance.
builder.Services.AddHttpClient<IAccountService, AccountService>(client =>
{
    client.BaseAddress = new Uri(baseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

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
