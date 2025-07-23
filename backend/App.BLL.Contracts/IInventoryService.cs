using Base.BLL.Contracts;

namespace App.BLL.Contracts;

public interface IInventoryService : IBaseService<App.BLL.DTO.Inventory>
{
    Task<IEnumerable<App.BLL.DTO.Inventory?>> GetEnrichedInventories();
}