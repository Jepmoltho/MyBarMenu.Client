using Blazored.LocalStorage;
using MyBarMenu.Client.Providers.Interfaces;

namespace MyBarMenu.Client.Handlers;

// You got to here: Rewrite to a http requesthandler which all http requests must go through so that the authToken is attatched to the request. 
// Figure out how it makes sense to distinquish between authorised endpoint request maybe with two different methods or handlers
public class HttpRequestHandler
{
    private readonly HttpClient _httpClient;
    private readonly ITokenProvider _tokenProvider;

    public HttpRequestHandler(HttpClient httpClient, ITokenProvider tokenProvider)
    {
        _httpClient = httpClient;
        _tokenProvider = tokenProvider;
    }

    public async Task<HttpResponseMessage> SendAsync(string path, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, path);

        var authToken = _tokenProvider.GetToken();  
        if (!string.IsNullOrWhiteSpace(authToken))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
        }

        return await _httpClient.SendAsync(request, cancellationToken);
    }

    //Post method 

    //Put 

    //Delete

    //Or specify "command" as a param eg get post put 
}
