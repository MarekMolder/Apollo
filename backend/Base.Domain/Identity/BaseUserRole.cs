using Microsoft.AspNetCore.Identity;

namespace Base.Domain.Identity;

/// <summary>
/// Abstract base class for the user-role relationship using <see cref="Guid"/> as the primary key.
/// </summary>
public abstract class BaseUserRole<TUser, TRole> : BaseUserRole<Guid, TUser, TRole>
    where TUser : class
    where TRole : class
{
}

/// <summary>
/// Abstract generic base class for the user-role relationship.
/// Extends <see cref="IdentityUserRole{TKey}"/> to include navigation properties for user and role.
/// </summary>
public abstract class BaseUserRole<TKey, TUser, TRole> : IdentityUserRole<TKey>
    where TKey : IEquatable<TKey>
    where TUser : class
    where TRole : class
{
    /// <summary>
    /// Navigation property to the associated user.
    /// </summary>
    public TUser? User { get; set; }
    
    /// <summary>
    /// Navigation property to the associated role.
    /// </summary>
    public TRole? Role { get; set; }
}