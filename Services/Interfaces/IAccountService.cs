using MyBarMenu.Client.DTOs;

namespace MyBarMenu.Client.Services.Interfaces;

public interface IAccountService
{
    Task<IEnumerable<UserDTO>> GetUsers();
}
