namespace App.DTO.v1.Identity;

/// <summary>
/// Data Transfer Object (DTO) for updating user profile fields.
/// Allows partial updates of profile information.
/// </summary>
public class UserFieldUpdate
{
    public string? FirstName { get; set; } = default!;
    public string? LastName { get; set; } = default!;
    public string? Username { get; set; } = default!;
    public string Email { get; set; } = default!;
}