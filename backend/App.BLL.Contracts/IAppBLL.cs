using Base.BLL.Contracts;

namespace App.BLL.Contracts;

public interface IAppBLL : IBaseBLL
{
    IActionEntityService ActionEntityService { get; }
    IActionTypeEntityService ActionTypeEntityService { get; }
    IAddressService AddressService { get; }
    ICurrentStockService CurrentStockService { get; }
    IInventoryService InventoryService { get; }
    IProductCategoryService ProductCategoryService { get; }
    IProductService ProductService { get; }
    IReasonService ReasonService { get; }
    IStorageRoomInInventoryService StorageRoomInInventoryService { get; }
    IStorageRoomService StorageRoomService { get; }
    ISupplierService SupplierService { get; }
    IPersonService PersonService { get; }
}