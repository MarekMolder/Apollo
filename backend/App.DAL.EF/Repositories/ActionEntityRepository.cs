using App.DAL.Contracts;
using App.DAL.EF.Mappers;
using App.Domain.Enums;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

/// <summary>
/// Repository implementation for accessing and querying ActionEntity data from the database.
/// Provides enriched data retrieval, as well as analytics on removed products and user activity.
/// </summary>
public class ActionEntityRepository : BaseRepository<DAL.DTO.ActionEntity, Domain.Logic.ActionEntity>, IActionEntityRepository
{
    public ActionEntityRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new ActionEntityUowMapper())
    {
    }
    
    /// <summary>
    /// Retrieves a domain-level ActionEntity by ID, including related ActionType.
    /// </summary>
    public async Task<Domain.Logic.ActionEntity?> FindAsync(Guid id)
    {
        return await RepositoryDbSet
            .Include(a => a.ActionType)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
    
    /// <summary>
    /// Returns all ActionEntities enriched with related entities.
    /// </summary>
    public async Task<IEnumerable<DAL.DTO.ActionEntity?>> GetEnrichedActionEntities()
    {
        var domainEntities = await RepositoryDbSet
            .Include(a => a.ActionType)
            .Include(a => a.Product)
            .Include(a => a.Reason)
            .Include(a => a.StorageRoom)
            .ToListAsync();

        return domainEntities.Select(e => Mapper.Map(e));
    }

    /// <summary>
    /// Returns the top 5 products with the highest total removed quantity.
    /// Filters only "Accepted" Remove-type actions.
    /// </summary>
    public async Task<List<(Guid ProductId, string ProductName, decimal RemoveQuantity, string ProductUnit, decimal? ProductVolume, string? ProductVolumeUnit)>> GetTopRemovedProductsAsync()
    {
        var results = await RepositoryDbSet
            .Include(a => a.Product) // et Product.Unit, Volume, VolumeUnit oleks olemas
            .Where(a =>
                a.Status == "Accepted" &&
                a.Product != null)
            .GroupBy(a => new 
            { 
                a.ProductId, 
                a.Product!.Name, 
                a.Product!.Unit, 
                ProductVolume = (decimal?)a.Product!.Volume, 
                ProductVolumeUnit = (string?)a.Product!.VolumeUnit 
            })
            .Select(g => new
            {
                g.Key.ProductId,
                ProductName = g.Key.Name,
                RemoveQuantity = g.Sum(x => x.Quantity),
                ProductUnit = g.Key.Unit,
                ProductVolume = g.Key.ProductVolume,
                ProductVolumeUnit = g.Key.ProductVolumeUnit
            })
            .OrderByDescending(x => x.RemoveQuantity)
            .Take(5)
            .ToListAsync();

        return results
            .Select(x => (
                x.ProductId, 
                x.ProductName, 
                x.RemoveQuantity, 
                x.ProductUnit, 
                x.ProductVolume, 
                x.ProductVolumeUnit
            ))
            .ToList();
    }

    
    /// <summary>
    /// Returns the top 5 users who have removed the most product quantity.
    /// Based on Remove-type actions with non-null CreatedBy.
    /// </summary>
    public async Task<List<(string CreatedBy, int TotalRemovals)>> GetTopUsersByRemovedQuantityAsync()
    {
        var result = await RepositoryDbSet
            .Include(a => a.ActionType)
            .Where(a => a.CreatedBy != null)
            .GroupBy(a => a.CreatedBy)
            .Select(g => new
            {
                CreatedBy = g.Key!,
                TotalRemovals = g.Count() // loendab kirjed, mitte kogust
            })
            .OrderByDescending(x => x.TotalRemovals)
            .Take(5)
            .ToListAsync();

        return result
            .Select(x => (x.CreatedBy, x.TotalRemovals))
            .ToList();
    }
    
}