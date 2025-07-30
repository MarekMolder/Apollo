using App.DAL.Contracts;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

/// <summary>
/// Repository implementation for accessing and querying MonthlyStatistics data from the database.
/// Provides enriched data retrieval, as well as analytics on removed products and user activity.
/// </summary>
public class MonthlyStatisticsRepository: BaseRepository<DAL.DTO.MonthlyStatistics, Domain.Logic.MonthlyStatistics>, IMonthlyStatisticsRepository
{
    public MonthlyStatisticsRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new MonthlyStatisticsUowMapper())
    {
    }
    
    /// <summary>
    /// Retrieves monthly statistics for a specific storage room, including related products.
    /// </summary>
    public async Task<IEnumerable<DAL.DTO.MonthlyStatistics?>> GetByStorageRoomIdAsync(Guid storageRoomId)
    {
        var domainEntities = await RepositoryDbSet
            .Include(x => x.Product)
            .Where(x => x.StorageRoomId == storageRoomId)
            .ToListAsync();

        return domainEntities.Select(e => Mapper.Map(e));
    }
    
    /// <summary>
    /// Finds a specific monthly statistics entry by product and storage room combination.
    /// </summary>
    public async Task<Domain.Logic.MonthlyStatistics?> FindByProductAndStorageAsync(Guid productId, Guid storageRoomId)
    {
        return await RepositoryDbSet
            .FirstOrDefaultAsync(c => c.ProductId == productId && c.StorageRoomId == storageRoomId);
    }
    
    /// <summary>
    /// Retrieves all monthly statistics entries, including related product and storage room details.
    /// </summary>
    public async Task<IEnumerable<DAL.DTO.MonthlyStatistics?>> GetEnrichedMonthlyStatistics()
    {
        var domainEntities = await RepositoryDbSet
            .Include(a => a.Product)
            .Include(a => a.StorageRoom)
            .ToListAsync();

        return domainEntities.Select(e => Mapper.Map(e));
    }
}