using Base.Contracts;

namespace Base.Domain;

/// <summary>
/// Abstract base entity class using <see cref="Guid"/> as the primary key.
/// Implements <see cref="IDomainId"/> and <see cref="IDomainMeta"/> for common metadata.
/// </summary>
public abstract class BaseEntity : BaseEntity<Guid>, IDomainId
{
}

/// <summary>
/// Abstract generic base entity class providing Id and metadata fields.
/// Implements <see cref="IDomainId{TKey}"/> and <see cref="IDomainMeta"/>.
/// </summary>
public abstract class BaseEntity<TKey>: IDomainId<TKey>, IDomainMeta
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// The primary key identifier for the entity.
    /// </summary>
    public TKey Id { get; set; } = default!;

    /// <summary>
    /// Username or system identifier of the user who created the entity.
    /// </summary>
    public string CreatedBy { get; set; } = default!;
    
    /// <summary>
    /// The UTC timestamp when the entity was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Username or system identifier of the user who last modified the entity.
    /// </summary>
    public string? ChangedBy { get; set; } = default!;
    
    /// <summary>
    /// The UTC timestamp when the entity was last modified.
    /// </summary>
    public DateTime? ChangedAt { get; set; }
    
    /// <summary>
    /// Optional system-level notes or internal comments for this entity.
    /// </summary>
    public string? SysNotes { get; set; }
}