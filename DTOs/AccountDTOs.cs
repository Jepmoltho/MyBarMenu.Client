using System.Text.Json.Serialization;

namespace MyBarMenu.Client.DTOs;

public class UserDTO
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("userName")]
    public string UserName { get; set; } = string.Empty;
}