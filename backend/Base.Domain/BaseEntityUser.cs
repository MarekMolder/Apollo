using Base.Contracts;

namespace Base.Domain;

/// <summary>
/// Abstract base class for entities that are owned or related to a user,
/// using <see cref="Guid"/> as the primary key.
/// </summary>
public abstract class BaseEntityUser<TUser> : BaseEntityUser<Guid, TUser>, IDomainId, IDomainUserId
    where TUser : class
{
}

/// <summary>
/// Abstract generic base class for entities that include a UserId and navigation property to a user.
/// Inherits from <see cref="BaseEntity{TKey}"/> and implements <see cref="IDomainUserId{TKey}"/>.
/// </summary>
public abstract class BaseEntityUser<TKey, TUser> : BaseEntity<TKey>, IDomainUserId<TKey>
    where TKey : IEquatable<TKey>
    where TUser : class
{
    /// <summary>
    /// Foreign key referencing the owning user.
    /// </summary>
    public TKey UserId { get; set; } = default!;
    
    /// <summary>
    /// Navigation property to the associated user entity.
    /// </summary>
    public TUser? User { get; set; }
}