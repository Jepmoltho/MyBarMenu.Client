using MyBarMenu.Client.DTOs;
using MyBarMenu.Client.Services.Interfaces;
using Blazored.LocalStorage;

namespace MyBarMenu.Client.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(HttpClient httpClient, ILocalStorageService localStorageService, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://localhost:7201/");
        _localStorageService = localStorageService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<UserDTO>> GetUsers()
    {
        //var authToken = _httpContextAccessor.HttpContext?.Session.GetString("authToken");
        //var request = new HttpRequestMessage(HttpMethod.Get, "users");
        //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);

        var response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "users"));

        if (response.IsSuccessStatusCode)
        {
            var users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();

            return users ?? new List<UserDTO>();
        } 
        else
        {
            return new List<UserDTO>();
        }
    }
}


//You got to here: Sending the auth token in the header is an additional layer of authorisation that authorises calls to endpoints with the [Authorize] tag. 
//You need to set it once for all http clients (as well as baseadress but this happens when building the application the first time)
//when logging in and remove the bearer token for all when logged out and notify the changes to the application. So that below code does not have to be
//replicated by each call.

//var authToken = await _localStorageService.GetItemAsync<string>("authToken");
//var request = new HttpRequestMessage(HttpMethod.Get, "users");
//    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
//    var response = await _httpClient.SendAsync(request);