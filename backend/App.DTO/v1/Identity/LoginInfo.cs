namespace App.DTO.v1.Identity;

/// <summary>
/// Data Transfer Object (DTO) representing login credentials for user authentication.
/// </summary>
public class LoginInfo
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}