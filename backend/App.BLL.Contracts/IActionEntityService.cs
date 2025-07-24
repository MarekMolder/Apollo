using Base.BLL.Contracts;

namespace App.BLL.Contracts;

public interface IActionEntityService : IBaseService<DTO.ActionEntity>
{
    Task<bool> UpdateStatusAsync(Guid id, string newStatus);
    Task<IEnumerable<DTO.ActionEntity?>> GetEnrichedActionEntities();
    Task<IEnumerable<(Guid ProductId, string ProductName, decimal RemoveQuantity)>> GetTopRemovedProductsAsync();
    Task<IEnumerable<(string CreatedBy, decimal TotalRemovedQuantity)>> GetTopUsersByRemovedQuantityAsync();
}