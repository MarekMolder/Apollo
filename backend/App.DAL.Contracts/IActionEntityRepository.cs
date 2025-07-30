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
    /// </summary>
    Task<List<(Guid ProductId, string ProductName, decimal RemoveQuantity)>> GetTopRemovedProductsAsync();

    /// <summary>
    /// Returns users who have removed the most quantity across all actions.
    /// </summary>
    Task<List<(string CreatedBy, decimal TotalRemovedQuantity)>> GetTopUsersByRemovedQuantityAsync();
}