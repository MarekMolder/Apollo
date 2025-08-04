using Microsoft.AspNetCore.Identity;

namespace Base.Domain.Identity;

/// <summary>
/// Abstract base class for application users using <see cref="Guid"/> as the primary key.
/// </summary>
public abstract class BaseUser<TUserRole> : BaseUser<Guid, TUserRole>
    where TUserRole : class
{
    
}

/// <summary>
/// Abstract generic base class for application users, extending <see cref="IdentityUser{TKey}"/>.
/// Includes navigation property for user-role relationships.
/// </summary>
public abstract class BaseUser<TKey, TUserRole>: IdentityUser<TKey>
    where TKey : IEquatable<TKey>
    where TUserRole : class
{
    /// <summary>
    /// Collection of user-role relationship entries associated with this user.
    /// </summary>
    public ICollection<TUserRole>? UserRoles { get; set; }
}