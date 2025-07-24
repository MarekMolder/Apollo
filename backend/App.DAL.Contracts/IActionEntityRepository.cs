using App.DAL.DTO;
using Base.DAL.Contracts;

namespace App.DAL.Contracts;

public interface IActionEntityRepository : IBaseRepository<ActionEntity>
{
    Task<Domain.Logic.ActionEntity?> FindAsync(Guid id);
    Task<IEnumerable<ActionEntity?>> GetEnrichedActionEntities();
    
    Task<List<(Guid ProductId, string ProductName, decimal RemoveQuantity)>> GetTopRemovedProductsAsync();

    Task<List<(string CreatedBy, decimal TotalRemovedQuantity)>> GetTopUsersByRemovedQuantityAsync();
    
}