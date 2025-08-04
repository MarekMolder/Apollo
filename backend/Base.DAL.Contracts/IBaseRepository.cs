using Base.Contracts;

namespace Base.DAL.Contracts;

/// <summary>
/// Base repository interface for entities with a <see cref="Guid"/> as the primary key.
/// </summary>
public interface IBaseRepository<TEntity> : IBaseRepository<TEntity, Guid>
    where TEntity : IDomainId
{
}

/// <summary>
/// Generic base repository interface providing CRUD operations for any entity type.
/// </summary>
public interface IBaseRepository<TEntity, TKey>
    where TEntity : IDomainId<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Returns all entities, optionally filtered by user ID.
    /// </summary>
    IEnumerable<TEntity> All(TKey? userId = default!);
    
    /// <summary>
    /// Asynchronously returns all entities, optionally filtered by user ID.
    /// </summary>
    Task<IEnumerable<TEntity>> AllAsync(TKey? userId = default!);
    
    /// <summary>
    /// Finds a specific entity by ID, optionally filtered by user ID.
    /// </summary>
    TEntity? Find(TKey id, TKey? userId = default!);
    
    /// <summary>
    /// Asynchronously finds a specific entity by ID, optionally filtered by user ID.
    /// </summary>
    Task<TEntity?> FindAsync(TKey id, TKey? userId = default!);

    /// <summary>
    /// Adds a new entity to the data store, optionally scoped by user ID.
    /// </summary>
    void Add(TEntity entity, TKey? userId = default!);
    
    /// <summary>
    /// Asynchronously adds a new entity to the data store, optionally scoped by user ID.
    /// </summary>
    Task AddAsync(TEntity entity, TKey? userId = default!);

    /// <summary>
    /// Updates an existing entity in the data store, optionally scoped by user ID.
    /// </summary>
    TEntity? Update(TEntity entity, TKey? userId = default!);
    
    /// <summary>
    /// Asynchronously updates an existing entity in the data store, optionally scoped by user ID.
    /// </summary>
    Task<TEntity?> UpdateAsync(TEntity entity, TKey? userId = default!);

    /// <summary>
    /// Removes an entity instance from the data store, optionally scoped by user ID.
    /// </summary>
    void Remove(TEntity entity, TKey? userId = default!);

    /// <summary>
    /// Removes an entity by ID from the data store, optionally scoped by user ID.
    /// </summary>
    void Remove(TKey id, TKey? userId = default!);
    
    /// <summary>
    /// Asynchronously removes an entity by ID from the data store, optionally scoped by user ID.
    /// </summary>
    Task RemoveAsync(TKey id, TKey? userId = default!);

    /// <summary>
    /// Checks if an entity with the given ID exists, optionally scoped by user ID.
    /// </summary>
    bool Exists(TKey id, TKey? userId = default!);
    
    /// <summary>
    /// Asynchronously checks if an entity with the given ID exists, optionally scoped by user ID.
    /// </summary>
    Task<bool> ExistsAsync(TKey id, TKey? userId = default!);
}