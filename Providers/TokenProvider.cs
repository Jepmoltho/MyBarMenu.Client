using Blazored.LocalStorage;
using MyBarMenu.Client.Providers.Interfaces;
using System.Runtime.CompilerServices;
namespace MyBarMenu.Client.Providers;

public class TokenProvider : ITokenProvider
{
    private string _cachedToken = string.Empty;

    public string GetToken() => _cachedToken;

    public void SetToken(string token) => _cachedToken = token;

    public void ClearToken() => _cachedToken = string.Empty; 
}
