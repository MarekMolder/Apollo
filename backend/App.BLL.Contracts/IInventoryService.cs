using Base.BLL.Contracts;

namespace App.BLL.Contracts;

public interface IInventoryService : IBaseService<DTO.Inventory>
{
    Task<IEnumerable<DTO.Inventory?>> GetEnrichedInventories();
}