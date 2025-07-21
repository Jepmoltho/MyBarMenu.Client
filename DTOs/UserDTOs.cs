namespace MyBarMenu.Client.DTOs;

public class UserInfo
{
    public bool IsAuthenticated { get; set; }
    public required string Email { get; set; }
}

public class UserResult : Result
{
    public Guid Id { get; set; } = Guid.Empty;
    public required string authToken { get; set; } = "";
}
