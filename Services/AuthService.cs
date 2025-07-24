using Blazored.LocalStorage;
using MyBarMenu.Client.Services.Interfaces;

namespace MyBarMenu.Client.Services;

public class AuthService : IAuthService
{
    private readonly ILocalStorageService _localStorage;
    private readonly CustomAuthStateProvider _authStateProvider;
    private readonly IHttpContextAccessor _httpContextAccessor; // Add IHttpContextAccessor
    
    public AuthService(ILocalStorageService localStorage, CustomAuthStateProvider authStateProvider, IHttpContextAccessor httpContextAccessor)
    {
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
        _httpContextAccessor = httpContextAccessor; // Initialize IHttpContextAccessor
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var authToken = await _localStorage.GetItemAsync<string>("authToken");
        return !string.IsNullOrWhiteSpace(authToken);
    }

    public async Task LoginAsync(string authToken)
    {
        await _localStorage.SetItemAsync("authToken", authToken);

        // Set session value only if session is available and not yet started
        //var httpContext = _httpContextAccessor.HttpContext;
        //if (httpContext != null && httpContext.Session != null && !httpContext.Response.HasStarted)
        //{
        //    httpContext.Session.SetString("authToken", authToken);
        //}

        _authStateProvider.NotifyUserAuthentification(authToken);
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("authToken");
        //var httpContext = _httpContextAccessor.HttpContext;
        //if (httpContext != null && httpContext.Session != null)
        //{
        //    httpContext.Session.Remove("authToken"); 
        //}
        _authStateProvider.NotifyUserLogout();
    }
}
