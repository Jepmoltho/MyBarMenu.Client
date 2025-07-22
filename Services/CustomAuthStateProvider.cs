using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace MyBarMenu.Client.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService;
    public CustomAuthStateProvider(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    //Called on pages with an [Authorize] tag
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

    //public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    //{
        
        
    //    var user = await _httpClient.GetFromJsonAsync<UserInfo>("user/auth");

    //    try
    //    {
    //        if (user is not null && user.IsAuthenticated)
    //        {
    //            var identity = new ClaimsIdentity(new[]
    //            {
    //            new Claim(ClaimTypes.Name, user.Email),
    //        }, "Cookies");

    //            return new AuthenticationState(new ClaimsPrincipal(identity));
    //        }
    //    }
    //    catch 
    //    { 
    //        //user is anonymous 
    //    }

    //    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    //}

    //public async Task<UserResult> SignInWithGoogle()
    //{
    //    var result = await _httpClient.GetFromJsonAsync<UserResult>("user/auth");

    //    if (result is not null && result.Success)
    //    {
    //        return result;
    //    }

    //    return new UserResult
    //    {
    //        Success = false,
    //        Message = "No user returned from API",
    //        authToken = string.Empty,
    //        Id = Guid.Empty,
    //    };
    //}
}
