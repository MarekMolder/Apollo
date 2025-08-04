namespace App.DTO.v1.Identity;

/// <summary>
/// Data Transfer Object (DTO) representing a successful JWT authentication response.
/// Contains the JWT, refresh token, user identity details, and assigned roles.
/// </summary>
public class JwtResponse
{
    /// <summary>
    /// Unique identifier of the authenticated user.
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// JSON Web Token (JWT) for authenticating subsequent API requests.
    /// </summary>
    public string Jwt { get; set; } = default!;
    
    /// <summary>
    /// Refresh token used to obtain a new JWT without re-authenticating.
    /// </summary>
    public string RefreshToken { get; set; } = default!;
    
    /// <summary>
    /// Email address of the authenticated user.
    /// </summary>
    public string Email { get; set; } = default!;
    
    /// <summary>
    /// List of roles assigned to the user (e.g., admin, user).
    /// </summary>
    public IEnumerable<string> Roles { get; set; } = new List<string>();
    
    /// <summary>
    /// First name of the authenticated user.
    /// </summary>
    public string FirstName { get; set; } = default!;
    
    /// <summary>
    /// Last name of the authenticated user.
    /// </summary>
    public string LastName { get; set; } = default!;
    
    /// <summary>
    /// Username of the authenticated user (usually same as email).
    /// </summary>
    public string Username { get; set; } = default!;
}