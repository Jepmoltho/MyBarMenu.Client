using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace MyBarMenu.Client.Components.Handlers;

// You got to here: Rewrite to a http requesthandler which all http requests must go through so that the authToken is attatched to the request. 
// Figure out how it makes sense to distinquish between authorised endpoint request maybe with two different methods or handlers
// Note that ILocalStorageService must be passed as a param as the server side application otherwise would not have access to browser
public class AuthTokenHandler
{
    private readonly HttpClient _httpClient;
    public AuthTokenHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> SendAsync(string path, ILocalStorageService _localStorageService, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, path);

        var authToken = await _localStorageService.GetItemAsync<string>("authToken");
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
