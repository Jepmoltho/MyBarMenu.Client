namespace MyBarMenu.Client.DTOs;

public class UserInfo
{
    public bool IsAuthenticated { get; set; }
    public required string Email { get; set; }
}
