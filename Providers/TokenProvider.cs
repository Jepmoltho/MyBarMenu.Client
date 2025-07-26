using Blazored.LocalStorage;
using MyBarMenu.Client.Providers.Interfaces;
using System.Runtime.CompilerServices;
namespace MyBarMenu.Client.Providers;

public class TokenProvider : ITokenProvider
{
    private string _cachedToken = string.Empty;

    //Consider writing as fake aync method to avoid blocking the UI thread. "Keeping it asynchronous allows the rest of your code (like HttpRequestHandler) to treat all token-fetching logic the same way, whether it’s cached in memory or fetched from an async source (like localStorage) — now or in the future."
    public string GetToken() => _cachedToken;

    public void SetToken(string token) => _cachedToken = token;

    public void ClearToken() => _cachedToken = string.Empty; 
}
