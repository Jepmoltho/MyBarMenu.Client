using Blazored.LocalStorage;
using MyBarMenu.Client.Providers;
using MyBarMenu.Client.Services.Interfaces;

namespace MyBarMenu.Client.Services;

public class AuthService : IAuthService
{
    private readonly ILocalStorageService _localStorage;
    private readonly CustomAuthStateProvider _authStateProvider;
    
    public AuthService(ILocalStorageService localStorage, CustomAuthStateProvider authStateProvider)
    {
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var authToken = await _localStorage.GetItemAsync<string>("authToken");
        return !string.IsNullOrWhiteSpace(authToken);
    }

    public async Task LoginAsync(string authToken)
    {
        await _localStorage.SetItemAsync("authToken", authToken);

        _authStateProvider.NotifyUserAuthentification(authToken);
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("authToken");

        _authStateProvider.NotifyUserLogout();
    }
}
