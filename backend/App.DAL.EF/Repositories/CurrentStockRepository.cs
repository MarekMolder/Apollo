using App.DAL.Contracts;
using App.DAL.DTO;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class CurrentStockRepository: BaseRepository<CurrentStock, Domain.Logic.CurrentStock>, ICurrentStockRepository
{
    public CurrentStockRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new CurrentStockUOWMapper())
    {
    }
    public async Task<Domain.Logic.CurrentStock?> FindByProductAndStorageAsync(Guid productId, Guid storageRoomId)
    {
        return await RepositoryDbSet
            .FirstOrDefaultAsync(c => c.ProductId == productId && c.StorageRoomId == storageRoomId);
    }
    
    public async Task<IEnumerable<CurrentStock?>> GetByStorageRoomIdAsync(Guid storageRoomId)
    {
        var domainEntities = await RepositoryDbSet
            .Include(x => x.Product)
            .Where(x => x.StorageRoomId == storageRoomId)
            .ToListAsync();

        return domainEntities.Select(e => Mapper.Map(e));
    }
    
    public async Task<List<(Guid ProductId, string ProductName, decimal Quantity)>> GetLowestStockProductsAsync(int count)
    {
        var result = await RepositoryDbSet
            .Include(cs => cs.Product)
            .OrderBy(cs => cs.Quantity)
            .Take(count)
            .Select(cs => new
            {
                cs.ProductId,
                ProductName = cs.Product!.Name,
                cs.Quantity
            })
            .ToListAsync();

        return result.Select(x => (x.ProductId, x.ProductName, x.Quantity)).ToList();
    }
    
    public async Task<decimal> GetTotalStorageRoomWorthAsync(Guid? storageRoomId = null)
    {
        var baseQuery = RepositoryDbSet
            .Include(cs => cs.Product)
            .Include(cs => cs.StorageRoom)
            .AsQueryable();

        if (storageRoomId != null)
        {
            baseQuery = baseQuery.Where(cs =>
                cs.StorageRoom != null && cs.StorageRoom.Id == storageRoomId);
        }

        return await baseQuery
            .Where(cs => cs.Product != null)
            .Select(cs => cs.Product!.Price * cs.Quantity)
            .SumAsync();
    }


}