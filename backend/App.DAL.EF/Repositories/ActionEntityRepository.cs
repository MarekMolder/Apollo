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
            .Include(a => a.StorageRoom)
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
    /// Filters only "Accepted" actions. 
    /// Optionally restricts to StorageRooms matching the given roles.
    /// </summary>
    public async Task<List<(Guid ProductId, string ProductName, decimal RemoveQuantity, string ProductUnit, decimal? ProductVolume, string? ProductVolumeUnit)>>
        GetTopRemovedProductsAsync(IEnumerable<string>? restrictToStorageRoles = null)
    {
        var query = RepositoryDbSet
            .Include(a => a.Product)
            .Include(a => a.StorageRoom)
            .Where(a => a.Status == "Accepted" && a.Product != null);
        
        var all = await query.ToListAsync();
        
        if (restrictToStorageRoles != null && restrictToStorageRoles.Any())
        {
            all = all
                .Where(a => a.StorageRoom?.AllowedRoles != null &&
                            a.StorageRoom.AllowedRoles.Intersect(restrictToStorageRoles, StringComparer.OrdinalIgnoreCase).Any())
                .ToList();
        }

        var results = all
            .GroupBy(a => new
            {
                a.ProductId,
                a.Product!.Name,
                a.Product!.Unit,
                ProductVolume = (decimal?)a.Product!.Volume,
                ProductVolumeUnit = (string?)a.Product!.VolumeUnit
            })
            .Select(g => (
                g.Key.ProductId,
                ProductName: g.Key.Name,
                RemoveQuantity: g.Sum(x => x.Quantity),
                ProductUnit: g.Key.Unit,
                ProductVolume: g.Key.ProductVolume,
                ProductVolumeUnit: g.Key.ProductVolumeUnit
            ))
            .OrderByDescending(x => x.RemoveQuantity)
            .Take(5)
            .ToList();

        return results;
    }
    
    /// <summary>
    /// Returns the top 5 users who have removed the most products (count of actions).
    /// Optionally restricts to StorageRooms matching the given roles.
    /// </summary>
    public async Task<List<(string CreatedBy, int TotalRemovals)>> 
        GetTopUsersByRemovedQuantityAsync(IEnumerable<string>? restrictToStorageRoles = null)
    {
        var query = RepositoryDbSet
            .Include(a => a.StorageRoom)
            .Where(a => a.Status == "Accepted" && a.CreatedBy != null);

        var all = await query.ToListAsync();

        if (restrictToStorageRoles != null && restrictToStorageRoles.Any())
        {
            all = all
                .Where(a => a.StorageRoom?.AllowedRoles != null &&
                            a.StorageRoom.AllowedRoles
                                .Intersect(restrictToStorageRoles, StringComparer.OrdinalIgnoreCase)
                                .Any())
                .ToList();
        }

        var result = all
            .GroupBy(a => a.CreatedBy!)
            .Select(g => (
                CreatedBy: g.Key,
                TotalRemovals: g.Sum(x => (int)x.Quantity)
            ))
            .OrderByDescending(x => x.TotalRemovals)
            .Take(5)
            .ToList();
        
        return result;
    }
}