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

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return new List<UserDTO>(); //ENDS IN HERE.
        }

        return new List<UserDTO>();
        //throw new Exception($"Failed to fetch users. Status code: {response.StatusCode}");
    }

    public async Task<UserResult> SignInWithGoogle()
    {
        var result = await _httpClient.GetFromJsonAsync<UserResult>("user/auth");

        if (result is not null && result.Success)
        {
            return result;
        }

        return new UserResult
        {
            Success = false,
            Message = "No user returned from API",
            authToken = string.Empty,
            Id = Guid.Empty,
        };
    }
}
