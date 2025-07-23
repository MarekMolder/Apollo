using Base.BLL.Contracts;

namespace App.BLL.Contracts;

public interface ICurrentStockService : IBaseService<App.BLL.DTO.CurrentStock>
{
    Task<IEnumerable<App.BLL.DTO.CurrentStock?>> GetByStorageRoomIdAsync(Guid storageRoomId);
    
    Task<List<(Guid ProductId, string ProductName, decimal Quantity)>> GetLowestStockProductsAsync(int count);
    
    Task<decimal> GetTotalInventoryWorthAsync(Guid? inventoryId = null);
}