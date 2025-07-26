using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using MyBarMenu.Client.Providers.Interfaces;
namespace MyBarMenu.Client.Providers;

// <summary>
// CustomAuthStateProvider is responsible for managing the authentication state of the user in a Blazor application.
// It retrieves the authentication token from local storage, parses it to create a ClaimsPrincipal, and notifies the application of the user's
// authentication state. GetAuthenticationStateAsync is the first wall of defence to be called pages that require authentication.
// If a user is authenticated it will return a ClaimsPrincipal with the user's claims and cache the auth token in the ITokenProvider to be
// retrived by the HttpRequestHandler for attaching the token to the header of http requests made on the page that is loaded. Hence authentication
// happens both frontend on a page level with GetAuthenticationStateAsync and backend with the [Authorize] tag on restricted endpoints.
// <summary>
public class CustomAuthStateProvider : AuthenticationStateProvider, ICustomAuthStateProvider
{
    private readonly ILocalStorageService _localStorageService;
    private readonly ITokenProvider _tokenProvider;

    public CustomAuthStateProvider(ILocalStorageService localStorageService, ITokenProvider tokenProvider)
    {
        _localStorageService = localStorageService;
        _tokenProvider = tokenProvider;
    }

    /// <summary>
    /// Implementation of GetAuthenticationStateAsync method that retrieves the authentication state of the user. 
    /// Will return an anonymous user if no auth token is found in local storage represented by an empty ClaimsPrincipal.
    /// Should be called in @code block of pages that require authentication. Gets the authToken from local storage because 
    /// that is persisted across page reloads and then caches it to ITokenProvider which is used by the HttpRequestHandler 
    /// to attach the token to the header of individual http requests made on the page that is loaded.
    /// </summary>
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var authToken = await _localStorageService.GetItemAsync<string>("authToken");

        if (string.IsNullOrWhiteSpace(authToken) || IsJwtTokenExpired(authToken)) 
        {
            _tokenProvider.ClearToken();
            await _localStorageService.RemoveItemAsync("authToken");

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        _tokenProvider.SetToken(authToken);
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
        _tokenProvider.SetToken(authToken);
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
        _tokenProvider.ClearToken();
        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string authToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(authToken);

        return token.Claims;
    }

    private static bool IsJwtTokenExpired(string authToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(authToken);

        var expiry = jwtToken.ValidTo;
        return expiry < DateTime.UtcNow;
    }
}
