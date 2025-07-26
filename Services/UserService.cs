using MyBarMenu.Client.DTOs;
using MyBarMenu.Client.Services.Interfaces;
using MyBarMenu.Client.Handlers;

namespace MyBarMenu.Client.Services;

public class UserService : IUserService
{
    private readonly HttpRequestHandler _httpRequestHandler;

    public UserService(HttpRequestHandler authTokenHandler)
    {
        _httpRequestHandler = authTokenHandler;
    }

    public async Task<List<UserDTO>> GetUsers()
    {
        var response = await _httpRequestHandler.SendAsync("/users", CancellationToken.None);

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