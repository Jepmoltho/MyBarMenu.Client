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
        var request = new HttpRequestMessage(HttpMethod.Get, "users");

        // (optional) Inspect and modify headers
        // For example, check if a cookie header exists
        //var cookies = request.Headers.TryGetValues("Cookie", out var cookieValues)
        //    ? string.Join("; ", cookieValues)
        //    : "No Cookie header set";

        //var cookie = _httpContextAccessor.HttpContext?.Request.Cookies[".AspNetCore.Identity.Application"];

        //var request = new HttpRequestMessage(HttpMethod.Get, "users");
        //request.Headers.Add("Cookie", $".AspNetCore.Identity.Application={cookie}");

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
