using Microsoft.AspNetCore.Components.Authorization;
using MyBarMenu.Client.DTOs;
using System.Security.Claims;

namespace MyBarMenu.Client.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;

    public CustomAuthStateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = await _httpClient.GetFromJsonAsync<UserInfo>("user/auth");

        try
        {
            if (user is not null && user.IsAuthenticated)
            {
                var identity = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, user.Email),
            }, "Cookies");

                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
        }
        catch 
        { 
            //user is anonymous 
        }

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    } 
}
