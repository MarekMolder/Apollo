using Base.BLL.Contracts;

namespace App.BLL.Contracts;

/// <summary>
/// Business logic contract for managing ActionEntity operations.
/// </summary>
public interface IActionEntityService : IBaseService<BLL.DTO.ActionEntity>
{
    /// <summary>
    /// Updates the status of an ActionEntity (e.g. to "Accepted" or "Declined").
    /// </summary>
    Task<bool> UpdateStatusAsync(Guid id, string newStatus);
    
    /// <summary>
    /// Retrieves ActionEntities enriched with related data.
    /// </summary>
    Task<IEnumerable<BLL.DTO.ActionEntity?>> GetEnrichedActionEntities();
    
    /// <summary>
    /// Retrieves ActionEntities enriched with related data + meta data.
    /// </summary>
    Task<IEnumerable<BLL.DTO.ActionEntity?>> GetEnrichedActionEntitiesFiltered(
        string? userEmail, int? month, int? year, string? status);
    
    /// <summary>
    /// Returns a list of products that have had the highest total quantity removed.
    /// </summary>
    Task<List<(Guid ProductId, string ProductName, decimal RemoveQuantity, string ProductUnit, decimal? ProductVolume, string? ProductVolumeUnit)>> 
        GetTopRemovedProductsAsync(IEnumerable<string>? restrictToStorageRoles = null);
    
    /// <summary>
    /// Returns a list of users who have removed the highest total quantity of products.
    /// </summary>
    Task<List<(string CreatedBy, int TotalRemovals)>> 
        GetTopUsersByRemovedQuantityAsync(IEnumerable<string>? restrictToStorageRoles = null);
}
