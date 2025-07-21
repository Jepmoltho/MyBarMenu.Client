using Microsoft.AspNetCore.Components.Authorization;
using MyBarMenu.Client.DTOs;
using System.Security.Claims;
using DotNetEnv;
using System;

namespace MyBarMenu.Client.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;

    public CustomAuthStateProvider(HttpClient httpClient)
    {
        var backendUrl = "https://localhost:7201"; // Environment.GetEnvironmentVariable("BACKEND_URL") ?? throw new InvalidOperationException("Backend url not set");

        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(backendUrl);
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
