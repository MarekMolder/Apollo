using Base.BLL.Contracts;

namespace App.BLL.Contracts;

public interface IAppBLL : IBaseBLL
{
    IActionEntityService ActionEntityService { get; }
    IActionTypeEntityService ActionTypeEntityService { get; }
    IAddressService AddressService { get; }
    ICurrentStockService CurrentStockService { get; }
    IProductCategoryService ProductCategoryService { get; }
    IProductService ProductService { get; }
    IReasonService ReasonService { get; }
    IStorageRoomService StorageRoomService { get; }
    ISupplierService SupplierService { get; }
    IMonthlyStatisticsService MonthlyStatisticsService { get; }
    IRecipeComponentService RecipeComponentService { get; }
}