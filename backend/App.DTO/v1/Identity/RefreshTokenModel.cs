namespace App.DTO.v1.Identity;

/// <summary>
/// Data Transfer Object (DTO) used for requesting a new JWT using a valid refresh token.
/// </summary>
public class RefreshTokenModel
{
    public string Jwt { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}