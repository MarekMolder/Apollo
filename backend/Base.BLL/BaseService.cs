using Base.BLL.Contracts;
using Base.Contracts;
using Base.DAL.Contracts;

namespace Base.BLL;

public class BaseService<TBllEntity, TDalEntity, TDalRepository> : BaseService<TBllEntity, TDalEntity, TDalRepository, Guid>, IBaseService<TBllEntity>
    where TBllEntity : class, IDomainId
    where TDalEntity : class, IDomainId
    where TDalRepository: class, IBaseRepository<TDalEntity>

{
    public BaseService(IBaseUOW serviceUOW, TDalRepository serviceRepository, IMapper<TBllEntity, TDalEntity, Guid> mapperMonthlyStatistics) : base(serviceUOW, serviceRepository, mapperMonthlyStatistics)
    {
    }
}

public class BaseService<TBllEntity, TDalEntity, TDalRepository, TKey>: IBaseService<TBllEntity, TKey>
    where TBllEntity : class, IDomainId<TKey>
    where TDalEntity : class, IDomainId<TKey>
    where TDalRepository: class, IBaseRepository<TDalEntity, TKey>
    where TKey : IEquatable<TKey>
{
    protected IBaseUOW ServiceUOW;
    protected TDalRepository ServiceRepository;
    protected IMapper<TBllEntity, TDalEntity, TKey> MapperMonthlyStatistics;


    public BaseService(IBaseUOW serviceUOW, TDalRepository serviceRepository, IMapper<TBllEntity, TDalEntity, TKey> mapperMonthlyStatistics)
    {
        ServiceUOW = serviceUOW;
        ServiceRepository = serviceRepository;
        MapperMonthlyStatistics = mapperMonthlyStatistics;
    }
    
    

    public virtual IEnumerable<TBllEntity> All(TKey? userId = default)
    {
        var entities = ServiceRepository.All(userId);
        return entities.Select(e => MapperMonthlyStatistics.Map(e)!).ToList(); 
    }

    public virtual async Task<IEnumerable<TBllEntity>> AllAsync(TKey? userId = default)
    {
        var entities = await ServiceRepository.AllAsync(userId);
        return entities.Select(e => MapperMonthlyStatistics.Map(e)!).ToList(); 
    }

    public virtual TBllEntity? Find(TKey id, TKey? userId = default)
    {
        var entity = ServiceRepository.Find(id, userId);
        return MapperMonthlyStatistics.Map(entity);
    }

    public virtual async Task<TBllEntity?> FindAsync(TKey id, TKey? userId = default)
    {
        var entity = await ServiceRepository.FindAsync(id, userId);
        return MapperMonthlyStatistics.Map(entity);
    }

    public virtual void Add(TBllEntity entity, TKey? userId = default)
    {
        var dalEntity = MapperMonthlyStatistics.Map(entity);
        ServiceRepository.Add(dalEntity!, userId);
    }

    public virtual async Task AddAsync(TBllEntity entity, TKey? userId = default)
    {
        var dalEntity = MapperMonthlyStatistics.Map(entity);
        await ServiceRepository.AddAsync(dalEntity!, userId);
    }

    public virtual TBllEntity? Update(TBllEntity entity, TKey? userId = default)
    {
        var dalEntity = MapperMonthlyStatistics.Map(entity);
        var updatedEntity = ServiceRepository.Update(dalEntity!, userId);
        return MapperMonthlyStatistics.Map(updatedEntity);
    }
    
    public virtual async Task<TBllEntity?> UpdateAsync(TBllEntity entity, TKey? userId = default)
    {
        var dalEntity = MapperMonthlyStatistics.Map(entity);
        var updatedEntity = await ServiceRepository.UpdateAsync(dalEntity!, userId);
        return MapperMonthlyStatistics.Map(updatedEntity);
    }

    public virtual void Remove(TBllEntity entity, TKey? userId = default)
    {
        Remove(entity.Id, userId);
    }

    public virtual void Remove(TKey id, TKey? userId = default)
    {
        var entity = ServiceRepository.Find(id, userId);
        if (entity != null)
        {
            ServiceRepository.Remove(entity, userId);
        }
    }

    public virtual async Task RemoveAsync(TKey id, TKey? userId = default)
    {
        var entity = await ServiceRepository.FindAsync(id, userId);
        if (entity != null)
        {
            await ServiceRepository.RemoveAsync(id, userId);
        }
    }

    public virtual bool Exists(TKey id, TKey? userId = default)
    {
        var entity = ServiceRepository.Find(id, userId);
        return entity != null;
    }

    public virtual async Task<bool> ExistsAsync(TKey id, TKey? userId = default)
    {
        var entity = await ServiceRepository.FindAsync(id, userId);
        return entity != null;
    }
}