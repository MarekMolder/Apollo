using Base.BLL.Contracts;
using Base.Contracts;
using Base.DAL.Contracts;

namespace Base.BLL;

/// <summary>
/// Base BLL service with Guid as default key type.
/// </summary>
public class BaseService<TBllEntity, TDalEntity, TDalRepository> : BaseService<TBllEntity, TDalEntity, TDalRepository, Guid>, IBaseService<TBllEntity>
    where TBllEntity : class, IDomainId
    where TDalEntity : class, IDomainId
    where TDalRepository: class, IBaseRepository<TDalEntity>

{
    public BaseService(IBaseUow serviceUow, TDalRepository serviceRepository, IMapper<TBllEntity, TDalEntity, Guid> mapperMonthlyStatistics) : base(serviceUow, serviceRepository, mapperMonthlyStatistics)
    {
    }
}

/// <summary>
/// Generic base service for implementing BLL logic using a repository and mapper.
/// </summary>
public class BaseService<TBllEntity, TDalEntity, TDalRepository, TKey>: IBaseService<TBllEntity, TKey>
    where TBllEntity : class, IDomainId<TKey>
    where TDalEntity : class, IDomainId<TKey>
    where TDalRepository: class, IBaseRepository<TDalEntity, TKey>
    where TKey : IEquatable<TKey>
{
    protected IBaseUow ServiceUow;
    protected TDalRepository ServiceRepository;
    protected IMapper<TBllEntity, TDalEntity, TKey> MapperMonthlyStatistics;
    
    public BaseService(IBaseUow serviceUow, TDalRepository serviceRepository, IMapper<TBllEntity, TDalEntity, TKey> mapperMonthlyStatistics)
    {
        ServiceUow = serviceUow;
        ServiceRepository = serviceRepository;
        MapperMonthlyStatistics = mapperMonthlyStatistics;
    }
    
    /// <summary>
    /// Returns all entities.
    /// </summary>
    public virtual IEnumerable<TBllEntity> All(TKey? userId = default)
    {
        var entities = ServiceRepository.All(userId);
        return entities.Select(e => MapperMonthlyStatistics.Map(e)!).ToList(); 
    }

    /// <summary>
    /// Asynchronously returns all entities.
    /// </summary>
    public virtual async Task<IEnumerable<TBllEntity>> AllAsync(TKey? userId = default)
    {
        var entities = await ServiceRepository.AllAsync(userId);
        return entities.Select(e => MapperMonthlyStatistics.Map(e)!).ToList(); 
    }

    /// <summary>
    /// Finds an entity by ID.
    /// </summary>
    public virtual TBllEntity? Find(TKey id, TKey? userId = default)
    {
        var entity = ServiceRepository.Find(id, userId);
        return MapperMonthlyStatistics.Map(entity);
    }

    /// <summary>
    /// Asynchronously finds an entity by ID.
    /// </summary>
    public virtual async Task<TBllEntity?> FindAsync(TKey id, TKey? userId = default)
    {
        var entity = await ServiceRepository.FindAsync(id, userId);
        return MapperMonthlyStatistics.Map(entity);
    }

    /// <summary>
    /// Adds a new entity.
    /// </summary>
    public virtual void Add(TBllEntity entity, TKey? userId = default)
    {
        var dalEntity = MapperMonthlyStatistics.Map(entity);
        ServiceRepository.Add(dalEntity!, userId);
    }

    /// <summary>
    /// Asynchronously adds a new entity.
    /// </summary>
    public virtual async Task AddAsync(TBllEntity entity, TKey? userId = default)
    {
        var dalEntity = MapperMonthlyStatistics.Map(entity);
        await ServiceRepository.AddAsync(dalEntity!, userId);
    }

    /// <summary>
    /// Updates an entity.
    /// </summary>
    public virtual TBllEntity? Update(TBllEntity entity, TKey? userId = default)
    {
        var dalEntity = MapperMonthlyStatistics.Map(entity);
        var updatedEntity = ServiceRepository.Update(dalEntity!, userId);
        return MapperMonthlyStatistics.Map(updatedEntity);
    }
    
    /// <summary>
    /// Asynchronously updates an entity.
    /// </summary>
    public virtual async Task<TBllEntity?> UpdateAsync(TBllEntity entity, TKey? userId = default)
    {
        var dalEntity = MapperMonthlyStatistics.Map(entity);
        var updatedEntity = await ServiceRepository.UpdateAsync(dalEntity!, userId);
        return MapperMonthlyStatistics.Map(updatedEntity);
    }

    /// <summary>
    /// Removes an entity.
    /// </summary>
    public virtual void Remove(TBllEntity entity, TKey? userId = default)
    {
        Remove(entity.Id, userId);
    }

    /// <summary>
    /// Removes an entity by ID.
    /// </summary>
    public virtual void Remove(TKey id, TKey? userId = default)
    {
        var entity = ServiceRepository.Find(id, userId);
        if (entity != null)
        {
            ServiceRepository.Remove(entity, userId);
        }
    }

    /// <summary>
    /// Asynchronously removes an entity by ID.
    /// </summary>
    public virtual async Task RemoveAsync(TKey id, TKey? userId = default)
    {
        var entity = await ServiceRepository.FindAsync(id, userId);
        if (entity != null)
        {
            await ServiceRepository.RemoveAsync(id, userId);
        }
    }

    /// <summary>
    /// Checks if entity exists.
    /// </summary>
    public virtual bool Exists(TKey id, TKey? userId = default)
    {
        var entity = ServiceRepository.Find(id, userId);
        return entity != null;
    }

    /// <summary>
    /// Asynchronously checks if entity exists.
    /// </summary>
    public virtual async Task<bool> ExistsAsync(TKey id, TKey? userId = default)
    {
        var entity = await ServiceRepository.FindAsync(id, userId);
        return entity != null;
    }
}