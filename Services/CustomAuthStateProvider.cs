using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
namespace MyBarMenu.Client.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService;

    private string AuthToken { get; set; } = string.Empty;

    public CustomAuthStateProvider(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    //Used to authorize access to pages. Must be called first thing in @code block and evaluate the authenticated state to conditionally render restricted content or redirect user back to login
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

    public void NotifyUserAuthentification(string authToken)
    {
        var identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken), "jwt");
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

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

    public void SetAuthToken(string authToken)
    {
        AuthToken = authToken;
        NotifyUserAuthentification(authToken);
    }

    public string GetAuthToken()
    {
        return AuthToken;
    }
}
