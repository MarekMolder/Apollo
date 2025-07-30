using App.DAL.Contracts;
using App.DAL.DTO;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class MonthlyStatisticsRepository: BaseRepository<MonthlyStatistics, Domain.Logic.MonthlyStatistics>, IMonthlyStatisticsRepository
{
    public MonthlyStatisticsRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new MonthlyStatisticsUOWMapper())
    {
    }
    
    public async Task<IEnumerable<MonthlyStatistics?>> GetByStorageRoomIdAsync(Guid storageRoomId)
    {
        var domainEntities = await RepositoryDbSet
            .Include(x => x.Product)
            .Where(x => x.StorageRoomId == storageRoomId)
            .ToListAsync();

        return domainEntities.Select(e => Mapper.Map(e));
    }
    
    public async Task<Domain.Logic.MonthlyStatistics?> FindByProductAndStorageAsync(Guid productId, Guid storageRoomId)
    {
        return await RepositoryDbSet
            .FirstOrDefaultAsync(c => c.ProductId == productId && c.StorageRoomId == storageRoomId);
    }
    
    public async Task<IEnumerable<MonthlyStatistics?>> GetEnrichedMonthlyStatistics()
    {
        var domainEntities = await RepositoryDbSet
            .Include(a => a.Product)
            .Include(a => a.StorageRoom)
            .ToListAsync();

        return domainEntities.Select(e => Mapper.Map(e));
    }
}