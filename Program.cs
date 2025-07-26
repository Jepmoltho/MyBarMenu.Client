using MyBarMenu.Client.Components;
using MyBarMenu.Client.Services;
using MyBarMenu.Client.Services.Interfaces;
using DotNetEnv;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using MyBarMenu.Client.Handlers;
using MyBarMenu.Client.Providers.Interfaces;
using MyBarMenu.Client.Providers;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables
Env.Load();
var baseUrl = Environment.GetEnvironmentVariable("BACKEND_URL") ?? throw new InvalidOperationException("Backend url not set");

// Add services to the container.
builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
builder.Services.AddAuthorizationCore();
builder.Services.AddAuthorization();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddHttpClient<HttpRequestHandler>(sp =>
{
    sp.BaseAddress = new Uri(baseUrl);
});
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddServerSideBlazor().AddCircuitOptions(options => {
    options.DetailedErrors = true;
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
