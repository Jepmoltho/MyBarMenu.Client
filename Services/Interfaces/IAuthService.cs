namespace MyBarMenu.Client.Services.Interfaces;

public interface IAuthService
{
    Task<bool> IsAuthenticatedAsync();
    Task LoginAsync(string token);
    Task LogoutAsync();
}
