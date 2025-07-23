using System.Linq.Expressions;
using App.DAL.Contracts;
using App.DAL.EF.Mappers;
using App.Domain.Logic;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class CurrentStockRepository: BaseRepository<App.DAL.DTO.CurrentStock, App.Domain.Logic.CurrentStock>, ICurrentStockRepository
{
    public CurrentStockRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new CurrentStockUOWMapper())
    {
    }
    public async Task<CurrentStock?> FindByProductAndStorageAsync(Guid productId, Guid storageRoomId)
    {
        return await RepositoryDbSet
            .FirstOrDefaultAsync(c => c.ProductId == productId && c.StorageRoomId == storageRoomId);
    }
    
    public async Task<IEnumerable<App.DAL.DTO.CurrentStock?>> GetByStorageRoomIdAsync(Guid storageRoomId)
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
                ProductId = cs.ProductId,
                ProductName = cs.Product!.Name,
                Quantity = cs.Quantity
            })
            .ToListAsync();

        return result.Select(x => (x.ProductId, x.ProductName, x.Quantity)).ToList();
    }
    
    public async Task<decimal> GetTotalInventoryWorthAsync(Guid? inventoryId = null)
    {
        var baseQuery = RepositoryDbSet
            .Include(cs => cs.Product)
            .Include(cs => cs.StorageRoom)
            .ThenInclude(sr => sr!.StorageRoomInInventories)
            .AsQueryable();

        if (inventoryId != null)
        {
            baseQuery = baseQuery.Where(cs =>
                cs.StorageRoom!.StorageRoomInInventories!
                    .Any(sri => sri.InventoryId == inventoryId));
        }

        return await baseQuery
            .Where(cs => cs.Product != null)
            .Select(cs => cs.Product!.Price * cs.Quantity)
            .SumAsync();
    }


}