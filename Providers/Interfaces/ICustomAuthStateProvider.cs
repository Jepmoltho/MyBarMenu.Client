using Microsoft.AspNetCore.Components.Authorization;

namespace MyBarMenu.Client.Providers.Interfaces;

public interface ICustomAuthStateProvider
{
    Task<AuthenticationState> GetAuthenticationStateAsync();
    void NotifyUserAuthentification(string authToken);
    void NotifyUserLogout();
}
