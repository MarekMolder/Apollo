using Base.BLL.Contracts;

namespace App.BLL.Contracts;

/// <summary>
/// Business Logic Layer (BLL) facade interface.
/// Provides access to all business services used in the application.
/// </summary>
public interface IAppBll : IBaseBll
{
    //ActionEntity Service
    IActionEntityService ActionEntityService { get; }
    
    //ActionTypeEntity Service
    IActionTypeEntityService ActionTypeEntityService { get; }
    
    //Address Service
    IAddressService AddressService { get; }
    
    //ProductCategory Service
    IProductCategoryService ProductCategoryService { get; }
    
    //Product Service
    IProductService ProductService { get; }
    
    //Reason Service
    IReasonService ReasonService { get; }
    
    //StoragRoom Service
    IStorageRoomService StorageRoomService { get; }
    
    //Supplier Service
    ISupplierService SupplierService { get; }
    
    //MonthlyStatistic Service
    IMonthlyStatisticsService MonthlyStatisticsService { get; }
    
    //RecipeComponent Service
    IRecipeComponentService RecipeComponentService { get; }
}