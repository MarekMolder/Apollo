using Base.DAL.Contracts;

namespace App.DAL.Contracts;

/// <summary>
/// Repository interface for handling data access related to ActionEntity.
/// </summary>
public interface IActionEntityRepository : IBaseRepository<DAL.DTO.ActionEntity>
{
    /// <summary>
    /// Finds an ActionEntity in domain format by its ID.
    /// </summary>
    Task<Domain.Logic.ActionEntity?> FindAsync(Guid id);
    
    /// <summary>
    /// Returns ActionEntities enriched with related data (joins from DB).
    /// </summary>
    Task<IEnumerable<DAL.DTO.ActionEntity?>> GetEnrichedActionEntities();
    
    /// <summary>
    /// Returns top products by removed quantity.
    /// If <paramref name="restrictToStorageRoles"/> is provided, 
    /// results will only include actions from StorageRooms whose AllowedRoles match.
    /// </summary>
    Task<List<(Guid ProductId, string ProductName, decimal RemoveQuantity, string ProductUnit, decimal? ProductVolume, string? ProductVolumeUnit)>> 
        GetTopRemovedProductsAsync(IEnumerable<string>? restrictToStorageRoles = null);

    /// <summary>
    /// Returns top users by number of remove-actions.
    /// If <paramref name="restrictToStorageRoles"/> is provided,
    /// results will only include actions from StorageRooms whose AllowedRoles match.
    /// </summary>
    Task<List<(string CreatedBy, int TotalRemovals)>> 
        GetTopUsersByRemovedQuantityAsync(IEnumerable<string>? restrictToStorageRoles = null);
}