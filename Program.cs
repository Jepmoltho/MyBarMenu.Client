using MyBarMenu.Client.Components;
using MyBarMenu.Client.Services;
using MyBarMenu.Client.Services.Interfaces;
using DotNetEnv;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using MyBarMenu.Client.Components.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables
Env.Load();
var baseUrl = Environment.GetEnvironmentVariable("BACKEND_URL") ?? throw new InvalidOperationException("Backend url not set");

// Add services to the container.
builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

// Registers AccountService as a scoped service. Each time it's injected, a new instance is created with its own HttpClient. The HttpClient is managed by IHttpClientFactory and scoped to the AccountService instance.

//register my AuthTokenHandler

builder.Services.AddHttpContextAccessor();
// Add this before builder.Build() to enable session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthorizationCore();
builder.Services.AddAuthorization();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthTokenHandler>();

builder.Services.AddHttpClient<AuthTokenHandler>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddServerSideBlazor().AddCircuitOptions(options => {
    options.DetailedErrors = true;
});


var app = builder.Build();
app.UseSession(); // Add this after app.UseHttpsRedirection()

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
