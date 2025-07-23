using App.Domain.Logic;
using Base.DAL.Contracts;

namespace App.DAL.Contracts;

public interface IActionEntityRepository : IBaseRepository<App.DAL.DTO.ActionEntity>
{
    Task<ActionEntity?> FindAsync(Guid id);
    Task<IEnumerable<App.DAL.DTO.ActionEntity?>> GetEnrichedActionEntities();
    
    Task<List<(Guid ProductId, string ProductName, decimal RemoveQuantity)>> GetTopRemovedProductsAsync();

    Task<List<(string CreatedBy, decimal TotalRemovedQuantity)>> GetTopUsersByRemovedQuantityAsync();
    
}