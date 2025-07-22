using MyBarMenu.Client.DTOs;
using MyBarMenu.Client.Services.Interfaces;
using Blazored.LocalStorage;

namespace MyBarMenu.Client.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;

    public UserService(HttpClient httpClient, ILocalStorageService localStorageService)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://localhost:7201/");
        _localStorageService = localStorageService;
    }

    public async Task<List<UserDTO>> GetUsers()
    {
        var authToken = await _localStorageService.GetItemAsync<string>("authToken");
        var request = new HttpRequestMessage(HttpMethod.Get, "users");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();

            return users ?? new List<UserDTO>();
        } 
        else
        {
            return new List<UserDTO>();
        }
        //var users = await _httpClient.GetFromJsonAsync<List<UserDTO>>("/users");

        //return users;
    }

    //public async Task<IEnumerable<UserDTO>> GetUsers()
    //{
    //    //var token = await _localStorageService.GetItemAsync<string>("authToken");

    //    if (string.IsNullOrEmpty(token))
    //    {
    //        return new List<UserDTO>();
    //    }

    //    //Im authorizing twice both in frontend and backend. Firstly on page level using AuthStateService and in every api call using [Authorize] and sending the token like this
    //    var request = new HttpRequestMessage(HttpMethod.Get, "users");
    //    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    //    var response = await _httpClient.SendAsync(request);

    //    //var response = await _httpClient.GetAsync("users");

    //    if (response.IsSuccessStatusCode)
    //    {
    //        var users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();

    //        return users ?? new List<UserDTO>();
    //    }

    //    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
    //    {
    //        throw new UnauthorizedAccessException();
    //        //return new List<UserDTO>(); //ENDS IN HERE.
    //    }

    //    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
    //    {
    //        return new List<UserDTO>();
    //    }

    //    return new List<UserDTO>();
    //    //throw new Exception($"Failed to fetch users. Status code: {response.StatusCode}");
    //}
}