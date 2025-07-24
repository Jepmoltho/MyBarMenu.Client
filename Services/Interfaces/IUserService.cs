using MyBarMenu.Client.DTOs;

namespace MyBarMenu.Client.Services.Interfaces;

public interface IUserService
{
    Task<List<UserDTO>> GetUsers();
}
