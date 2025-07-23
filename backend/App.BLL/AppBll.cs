using App.BLL.Contracts;
using App.BLL.Mappers;
using App.BLL.Services;
using App.DAL.Contracts;
using App.DAL.EF;
using App.DAL.EF.Mappers;
using Base.BLL;

namespace App.BLL;

public class AppBll: BaseBll<IAppUOW>, IAppBLL
{
    public AppBll(IAppUOW uow) : base(uow)
    {
    }

    private IActionEntityService? _actionEntityService;
    public IActionEntityService ActionEntityService => 
        _actionEntityService ??= new ActionEntityService(BLLUOW, new ActionEntityBLLMapper(), new CurrentStockUOWMapper(), new ActionEntityUOWMapper());
    
    private IActionTypeEntityService? _actionTypeEntityService;
    public  IActionTypeEntityService ActionTypeEntityService => 
        _actionTypeEntityService ??= new ActionTypeEntityService(BLLUOW, new ActionTypeEntityBLLMapper());
    
    private IAddressService? _addressService;
    public  IAddressService AddressService => 
        _addressService ??= new AddressService(BLLUOW, new AddressBLLMapper());
    
    private ICurrentStockService? _currentStockService;
    public  ICurrentStockService CurrentStockService => 
        _currentStockService ??= new CurrentStockService(BLLUOW, new CurrentStockBLLMapper());
    
    private IInventoryService? _inventoryService;
    public  IInventoryService InventoryService => 
        _inventoryService ??= new InventoryService(BLLUOW, new InventoryBLLMapper());
    
    private IProductCategoryService? _productCategoryService;
    public  IProductCategoryService ProductCategoryService => 
        _productCategoryService ??= new ProductCategoryService(BLLUOW, new ProductCategoryBLLMapper());
    
    private IProductService? _productService;
    public  IProductService ProductService => 
        _productService ??= new ProductService(BLLUOW, new ProductBLLMapper());
    
    private IReasonService? _reasonService;
    public  IReasonService ReasonService => 
        _reasonService ??= new ReasonService(BLLUOW, new ReasonBLLMapper());
    
    private IStockAuditService? _stockAuditService;
    public  IStockAuditService StockAuditService => 
        _stockAuditService ??= new StockAuditService(BLLUOW, new StockAuditBLLMapper());
    
    private IStockMovementService? _stockMovementService;
    public  IStockMovementService StockMovementService => 
        _stockMovementService ??= new StockMovementService(BLLUOW, new StockMovementBLLMapper());
    
    private IStorageRoomInInventoryService? _storageRoomInInventoryService;
    public  IStorageRoomInInventoryService StorageRoomInInventoryService => 
        _storageRoomInInventoryService ??= new StorageRoomInInventoryService(BLLUOW, new StorageRoomInInventoryBLLMapper());
    
    private IStorageRoomService? _storageRoomService;
    public  IStorageRoomService StorageRoomService => 
        _storageRoomService ??= new StorageRoomService(BLLUOW, new StorageRoomBLLMapper());
    
    private ISupplierService? _supplierService;
    public  ISupplierService SupplierService => 
        _supplierService ??= new SupplierService(BLLUOW, new SupplierBLLMapper());
    
    private IPersonService? _personService;
    public  IPersonService PersonService => 
        _personService ??= new PersonService(BLLUOW, new PersonBLLMapper());
}