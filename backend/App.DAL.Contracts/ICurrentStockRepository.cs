using System.Linq.Expressions;
using App.Domain.Logic;
using Base.DAL.Contracts;

namespace App.DAL.Contracts;

public interface ICurrentStockRepository: IBaseRepository<App.DAL.DTO.CurrentStock>
{
    Task<CurrentStock?> FindByProductAndStorageAsync(Guid productId, Guid storageRoomId);
    
    Task<IEnumerable<App.DAL.DTO.CurrentStock?>> GetByStorageRoomIdAsync(Guid storageRoomId);
    
    Task<List<(Guid ProductId, string ProductName, decimal Quantity)>> GetLowestStockProductsAsync(int count);
    
    Task<decimal> GetTotalInventoryWorthAsync(Guid? inventoryId = null);
}