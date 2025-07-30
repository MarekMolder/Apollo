using App.BLL.Contracts;
using App.BLL.Mappers;
using App.BLL.Services;
using App.DAL.Contracts;
using App.DAL.EF.Mappers;
using Base.BLL;

namespace App.BLL;

/// <summary>
/// Central Business Logic Layer (BLL) entry point that provides access to all BLL services.
/// Services are lazily instantiated and share a common Unit of Work.
/// </summary>
public class AppBll: BaseBll<IAppUOW>, IAppBll
{
    public AppBll(IAppUOW uow) : base(uow)
    {
    }

    //ActionEntity Service
    private IActionEntityService? _actionEntityService;
    public IActionEntityService ActionEntityService => 
        _actionEntityService ??= new ActionEntityService(BLLUOW, new ActionEntityBllMapper(), new MonthlyStatisticsUowMapper(), new ActionEntityUowMapper());
    
    //ActionTypeEntity Service
    private IActionTypeEntityService? _actionTypeEntityService;
    public  IActionTypeEntityService ActionTypeEntityService => 
        _actionTypeEntityService ??= new ActionTypeEntityService(BLLUOW, new ActionTypeEntityBllMapper());
    
    //Address Service
    private IAddressService? _addressService;
    public  IAddressService AddressService => 
        _addressService ??= new AddressService(BLLUOW, new AddressBllMapper());
    
    //ProductCategory Service
    private IProductCategoryService? _productCategoryService;
    public  IProductCategoryService ProductCategoryService => 
        _productCategoryService ??= new ProductCategoryService(BLLUOW, new ProductCategoryBllMapper());
    
    //Product Service
    private IProductService? _productService;
    public  IProductService ProductService => 
        _productService ??= new ProductService(BLLUOW, new ProductBllMapper());
    
    //Reason Service
    private IReasonService? _reasonService;
    public  IReasonService ReasonService => 
        _reasonService ??= new ReasonService(BLLUOW, new ReasonBllMapper());
    
    //StorageRoom Service
    private IStorageRoomService? _storageRoomService;
    public  IStorageRoomService StorageRoomService => 
        _storageRoomService ??= new StorageRoomService(BLLUOW, new StorageRoomBllMapper());
    
    //Supplier Service
    private ISupplierService? _supplierService;
    public  ISupplierService SupplierService => 
        _supplierService ??= new SupplierService(BLLUOW, new SupplierBllMapper());
    
    //MonthlyStatistics Service
    private IMonthlyStatisticsService? _monthlyStatisticsService;
    public  IMonthlyStatisticsService MonthlyStatisticsService => 
        _monthlyStatisticsService ??= new MonthlyStatisticsService(BLLUOW, new MonthlyStatisticsBllMapper());
    
    //RecipeComponent Service
    private IRecipeComponentService? _recipeComponentService;
    public  IRecipeComponentService RecipeComponentService => 
        _recipeComponentService ??= new RecipeComponentService(BLLUOW, new RecipeComponentBllMapper());
}
