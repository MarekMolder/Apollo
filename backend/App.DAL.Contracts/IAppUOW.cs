using Base.DAL.Contracts;

namespace App.DAL.Contracts;

public interface IAppUOW : IBaseUOW
{
    IActionEntityRepository ActionEntityRepository { get; }
    IActionTypeEntityRepository ActionTypeEntityRepository { get; }
    IAddressRepository AddressRepository { get; }
    ICurrentStockRepository CurrentStockRepository { get; }
    IInventoryRepository InventoryRepository { get; }
    IProductCategoryRepository ProductCategoryRepository { get; }
    IProductRepository ProductRepository { get; }
    IReasonRepository ReasonRepository { get; }
    IStorageRoomInInventoryRepository StorageRoomInInventoryRepository { get; }
    IStorageRoomRepository StorageRoomRepository { get; }
    ISupplierRepository SupplierRepository { get; }
    IPersonRepository PersonRepository { get; }
    
}