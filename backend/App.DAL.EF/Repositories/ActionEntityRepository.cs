using System.Security.Cryptography;
using App.DAL.Contracts;
using App.DAL.EF.Mappers;
using App.Domain.Logic;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ActionEntityRepository : BaseRepository<App.DAL.DTO.ActionEntity, App.Domain.Logic.ActionEntity>, IActionEntityRepository
{
    public ActionEntityRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new ActionEntityUOWMapper())
    {
    }
    
    public async Task<ActionEntity?> FindAsync(Guid id)
    {
        return await RepositoryDbSet
            .Include(a => a.ActionType)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
    public async Task<IEnumerable<App.DAL.DTO.ActionEntity?>> GetEnrichedActionEntities()
    {
        var domainEntities = await RepositoryDbSet
            .Include(a => a.ActionType)
            .Include(a => a.Product)
            .Include(a => a.Supplier)
            .Include(a => a.Reason)
            .Include(a => a.StorageRoom)
            .ToListAsync();

        return domainEntities.Select(e => Mapper.Map(e));
    }

    public async Task<List<(Guid ProductId, string ProductName, decimal RemoveQuantity)>> GetTopRemovedProductsAsync()
    {
        var results = await RepositoryDbSet
            .Where(a =>
                a.Status == "Accepted" &&
                a.ActionType!.Code == App.Domain.Enums.ActionTypeEnum.Remove &&
                a.Product != null)
            .GroupBy(a => new { a.ProductId, a.Product!.Name })
            .Select(g => new
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.Name,
                RemoveQuantity = g.Sum(x => x.Quantity)
            })
            .OrderByDescending(x => x.RemoveQuantity)
            .Take(5)
            .ToListAsync();

        return results
            .Select(x => (x.ProductId, x.ProductName, x.RemoveQuantity))
            .ToList();
    }
    
    public async Task<List<(string CreatedBy, decimal TotalRemovedQuantity)>> GetTopUsersByRemovedQuantityAsync()
    {
        var result = await RepositoryDbSet
            .Include(a => a.ActionType)
            .Where(a =>
                a.ActionType!.Code == App.Domain.Enums.ActionTypeEnum.Remove &&
                a.CreatedBy != null)
            .GroupBy(a => a.CreatedBy)
            .Select(g => new
            {
                CreatedBy = g.Key!,
                TotalRemovedQuantity = g.Sum(x => x.Quantity)
            })
            .OrderByDescending(x => x.TotalRemovedQuantity)
            .Take(5)
            .ToListAsync();

        return result
            .Select(x => (x.CreatedBy, x.TotalRemovedQuantity))
            .ToList();
    }

}