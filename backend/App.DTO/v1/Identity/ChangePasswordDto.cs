namespace App.DTO.v1.Identity;

/// <summary>
/// Data Transfer Object (DTO) for requesting a password change.
/// Used in user profile or security settings when a user wants to update their password.
/// </summary>
public class ChangePasswordDto
{
    public string Email { get; set; } = default!;
    public string CurrentPassword { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}