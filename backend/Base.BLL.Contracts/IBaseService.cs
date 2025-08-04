using Base.Contracts;
using Base.DAL.Contracts;

namespace Base.BLL.Contracts;

/// <summary>
/// Defines a generic base interface for BLL services with <c>Guid</c> as the default key type.
/// Combines business logic service operations with repository access.
/// </summary>
public interface IBaseService<TEntity> : IBaseService<TEntity, Guid>, IBaseRepository<TEntity>
where TEntity : IDomainId
{
    
}

/// <summary>
/// Defines a generic base interface for BLL services, supporting any key type.
/// Inherits repository-level functionality to allow data operations in BLL context.
/// </summary>
public interface IBaseService<TEntity, TKey> : IBaseRepository<TEntity, TKey>
    where TEntity : IDomainId<TKey>
    where TKey : IEquatable<TKey>
{
}