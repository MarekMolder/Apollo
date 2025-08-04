using Base.Contracts;
using Base.Domain.Identity;

namespace App.Domain.Identity;

/// <summary>
/// Domain entity representing a refresh token tied to an application user.
/// Inherits from BaseRefreshToken and includes a reference to the associated AppUser.
/// Used for implementing secure and persistent user authentication.
/// </summary>
public class AppRefreshToken : BaseRefreshToken, IDomainUserId
{
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
}