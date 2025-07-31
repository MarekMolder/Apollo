namespace Base.Contracts;

/// <summary>
/// Non-generic version of <see cref="IDomainUserId{TKey}"/> using <see cref="Guid"/> as key type.
/// </summary>
public interface IDomainUserId : IDomainUserId<Guid>
{
    
}

/// <summary>
/// Interface for domain entities that are associated with a specific user.
/// </summary>
public interface IDomainUserId<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Identifier of the user who owns this entity.
    /// </summary>
    public TKey UserId { get; set; }    
}