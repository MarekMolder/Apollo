namespace Base.Contracts;

/// <summary>
/// Non-generic version of <see cref="IDomainId{TKey}"/> using <see cref="Guid"/> as the default key type.
/// </summary>
public interface IDomainId : IDomainId<Guid>
{
    
}

/// <summary>
/// Interface for domain entities that have a primary identifier.
/// </summary>
public interface IDomainId<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// The unique identifier of the entity.
    /// </summary>
    public TKey Id { get; set; }
}