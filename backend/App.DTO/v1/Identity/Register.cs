using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1.Identity;

/// <summary>
/// Data Transfer Object (DTO) used for user registration.
/// Includes email, password, and basic user profile information.
/// </summary>
public class Register
{
    /// <summary>
    /// User's email address. Used for login and communication.
    /// </summary>
    public string Email { get; set; } = default!;

    /// <summary>
    /// User's chosen password.
    /// </summary>
    public string Password { get; set; } = default!;

    /// <summary>
    /// User's first name.
    /// </summary>
    public string FirstName { get; set; }= default!;
        
    /// <summary>
    /// User's last name.
    /// </summary>
    public string LastName { get; set; }= default!;
}