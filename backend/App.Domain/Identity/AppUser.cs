using System.ComponentModel.DataAnnotations;
using App.Domain.Logic;
using Base.Domain.Identity;

namespace App.Domain.Identity;

/// <summary>
/// Application-specific user entity extending the BaseUser with domain-specific fields.
/// Includes first and last name, refresh tokens for authentication, and related domain associations.
/// </summary>
public class AppUser : BaseUser<AppUserRole>
{
    /// <summary>
    /// User's first name (1–128 characters).
    /// </summary>
    [MinLength(1)]
    [MaxLength(128)]
    public string FirstName { get; set; } = default!;
    
    /// <summary>
    /// User's last name (1–128 characters).
    /// </summary>
    [MinLength(1)]
    [MaxLength(128)]
    public string LastName { get; set; } = default!;
    
    /// <summary>
    /// Refresh tokens issued to this user for authentication and session renewal.
    /// </summary>
    public ICollection<AppRefreshToken>? RefreshTokens { get; set; }
}