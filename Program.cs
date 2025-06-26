using MyBarMenu.Client.Components;
using MyBarMenu.Client.Services;
using MyBarMenu.Client.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<IAccountService, AccountService>();

//builder.Services.AddScoped(sp =>
//{
//    var baseUri = "https://my-bar-menu-api-gpeme8e7dvccbtcx.northeurope-01.azurewebsites.net/";
//    return new HttpClient
//    {
//        BaseAddress = new Uri(baseUri),
//        DefaultRequestHeaders = { { "Accept", "application/json" } }
//    };
//});

//builder.Services.AddScoped<IAccountService, AccountService>();

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
