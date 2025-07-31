namespace Base.Contracts;

/// <summary>
/// A base mapper interface for converting between upper and lower layer entities (e.g., BLL ↔ DAL).
/// </summary>
public interface IMapper<TUpperEntity, TLowerEntity> : IMapper<TUpperEntity, TLowerEntity, Guid>
    where TUpperEntity : class, IDomainId
    where TLowerEntity : class, IDomainId
{
}

/// <summary>
/// Generic mapper interface for converting between two domain entity types with a specified key type.
/// </summary>
public interface IMapper<TUpperEntity, TLowerEntity, TKey>
    where TKey : IEquatable<TKey>
    where TUpperEntity : class, IDomainId<TKey>
    where TLowerEntity : class, IDomainId<TKey>
{
    /// <summary>
    /// Maps an entity from the lower layer (e.g., DAL) to the upper layer (e.g., BLL).
    /// </summary>
    public TUpperEntity? Map(TLowerEntity? entity);
    
    /// <summary>
    /// Maps an entity from the upper layer (e.g., BLL) to the lower layer (e.g., DAL).
    /// </summary>
    public TLowerEntity? Map(TUpperEntity? entity);
}
