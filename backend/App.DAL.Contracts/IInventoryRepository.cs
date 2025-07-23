using App.Domain.Logic;
using Base.DAL.Contracts;

namespace App.DAL.Contracts;

public interface IInventoryRepository: IBaseRepository<App.DAL.DTO.Inventory>
{
    Task<IEnumerable<App.DAL.DTO.Inventory?>> GetEnrichedInventories();
}