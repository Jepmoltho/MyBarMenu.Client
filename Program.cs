using MyBarMenu.Client.Components;
using MyBarMenu.Client.Services;
using MyBarMenu.Client.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//var baseUri = "https://my-bar-menu-api-gpeme8e7dvccbtcx.northeurope-01.azurewebsites.net/";
var baseUri = "https://localhost:7201/";

// Registers AccountService as a scoped service. Each time it's injected, a new instance is created with its own HttpClient. The HttpClient is managed by IHttpClientFactory and scoped to the AccountService instance.
builder.Services.AddHttpClient<IAccountService, AccountService>(client =>
{
    client.BaseAddress = new Uri(baseUri);
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
