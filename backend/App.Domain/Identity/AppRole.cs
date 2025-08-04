using Base.Domain.Identity;

namespace App.Domain.Identity;

/// <summary>
/// Application-specific role entity that extends the BaseRole with a link to AppUserRole.
/// Used for defining role-based access control within the identity system.
/// </summary>
public class AppRole : BaseRole<AppUserRole>
{
    
}