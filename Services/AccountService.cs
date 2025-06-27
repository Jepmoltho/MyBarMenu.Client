using MyBarMenu.Client.DTOs;
using MyBarMenu.Client.Services.Interfaces;

namespace MyBarMenu.Client.Services;

public class AccountService : IAccountService
{
    private readonly HttpClient _httpClient;

    public AccountService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<UserDTO>> GetUsers()
    {
        var response = await _httpClient.GetAsync("users");

        if (response.IsSuccessStatusCode)
        {
            var users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();

            return users ?? new List<UserDTO>();
        }

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return new List<UserDTO>();
        }

        throw new Exception($"Failed to fetch users. Status code: {response.StatusCode}");
    }
}
