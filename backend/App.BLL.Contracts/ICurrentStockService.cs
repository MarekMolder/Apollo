using Base.BLL.Contracts;

namespace App.BLL.Contracts;

public interface ICurrentStockService : IBaseService<DTO.CurrentStock>
{
    Task<IEnumerable<DTO.CurrentStock?>> GetByStorageRoomIdAsync(Guid storageRoomId);
    
    Task<List<(Guid ProductId, string ProductName, decimal Quantity)>> GetLowestStockProductsAsync(int count);
    
    Task<decimal> GetTotalStorageRoomWorthAsync(Guid? storageRoomId = null);
}