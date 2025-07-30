using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1.Identity;

/// <summary>
/// Data Transfer Object (DTO) used to request logout by invalidating the refresh token.
/// </summary>
public class LogoutInfo
{
    [Required]
    public string RefreshToken { get; set; } = default!;
}