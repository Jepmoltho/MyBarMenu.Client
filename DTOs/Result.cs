namespace MyBarMenu.Client.DTOs;

public class Result
{
    public bool Success { get; set; }
    public required string Message { get; set; } = string.Empty;
}
