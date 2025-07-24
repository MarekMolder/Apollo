using App.DAL.DTO;
using Base.DAL.Contracts;

namespace App.DAL.Contracts;

public interface ICurrentStockRepository: IBaseRepository<CurrentStock>
{
    Task<Domain.Logic.CurrentStock?> FindByProductAndStorageAsync(Guid productId, Guid storageRoomId);
    
    Task<IEnumerable<CurrentStock?>> GetByStorageRoomIdAsync(Guid storageRoomId);
    
    Task<List<(Guid ProductId, string ProductName, decimal Quantity)>> GetLowestStockProductsAsync(int count);
    
    Task<decimal> GetTotalInventoryWorthAsync(Guid? inventoryId = null);
}