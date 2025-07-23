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
    }
}