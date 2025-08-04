using Microsoft.AspNetCore.Identity;

namespace Base.Domain.Identity;

/// <summary>
/// Abstract base class for application roles using <see cref="Guid"/> as the primary key.
/// </summary>
public abstract class BaseRole<TUserRole> : BaseRole<Guid, TUserRole>
    where TUserRole : class
{
}

/// <summary>
/// Abstract generic base class for application roles, extending <see cref="IdentityRole{TKey}"/>.
/// Includes navigation property for user-role relationships.
/// </summary>
public abstract class BaseRole<TKey, TUserRole> : IdentityRole<TKey>
    where TKey : IEquatable<TKey>
    where TUserRole : class
{
    /// <summary>
    /// Collection of user-role relationship entries associated with this role.
    /// </summary>
    public ICollection<TUserRole>? UserRoles { get; set; }
}