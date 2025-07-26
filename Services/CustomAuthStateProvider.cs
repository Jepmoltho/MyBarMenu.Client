using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
namespace MyBarMenu.Client.Services;

// <summary>
// CustomAuthStateProvider is responsible for managing the authentication state of the user in a Blazor application.
// It retrieves the authentication token from local storage, parses it to create a ClaimsPrincipal, and notifies the application of the user's authentication state.
// GetAuthenticationStateAsync is the first wall of defence to be called pages that require authentication.
// Not to be confused with the authTokenHandler that attaches jwt auth token to the header of http requests which is the second wall of defence.
// <summary>
public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService;

    public CustomAuthStateProvider(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    /// <summary>
    /// Implementation of GetAuthenticationStateAsync method that retrieves the authentication state of the user.
    /// Should be called in @code block of pages that require authentication.
    /// </summary>
    /// <returns></returns>
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var authToken = await _localStorageService.GetItemAsync<string>("authToken");

        if (string.IsNullOrWhiteSpace(authToken)) 
        {
            //Represents an unauthorised/anonymous user
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken), "jwt");
        var user = new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }

    /// <summary>
    /// Uses the auth token to create a ClaimsIdentity and notify the application of the authenticated user when the user logs in.
    /// </summary>
    /// <param name="authToken"></param>
    public void NotifyUserAuthentification(string authToken)
    {
        var identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken), "jwt");
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    /// <summary>
    /// Notifies the system that the current user has logged out and updates the authentication state.
    /// </summary>
    /// <remarks>This method sets the authentication state to an anonymous user, effectively logging out the
    /// current user. It triggers the <see cref="NotifyAuthenticationStateChanged"/> event to inform subscribers of the
    /// state change.</remarks>
    public void NotifyUserLogout()
    {
        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string authToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(authToken);

        return token.Claims;
    }
}
