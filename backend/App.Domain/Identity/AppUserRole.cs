using Base.Domain.Identity;

namespace App.Domain.Identity;

/// <summary>
/// Join entity that defines the many-to-many relationship between AppUser and AppRole.
/// Inherits from BaseUserRole to provide identity role linking functionality.
/// </summary>
public class AppUserRole : BaseUserRole<AppUser, AppRole>
{
    
}