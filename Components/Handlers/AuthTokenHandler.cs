using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace MyBarMenu.Client.Components.Handlers
{
    public class AuthTokenHandler
    {
        private readonly HttpClient _httpClient;
        public AuthTokenHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
            //_httpClient.BaseAddress = new Uri("https://localhost:7201/");
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
    }
}
