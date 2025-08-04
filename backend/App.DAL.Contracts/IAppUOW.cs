using Base.DAL.Contracts;

namespace App.DAL.Contracts;

public interface IAppUOW : IBaseUow
{
    IActionEntityRepository ActionEntityRepository { get; }
    IActionTypeEntityRepository ActionTypeEntityRepository { get; }
    IAddressRepository AddressRepository { get; }
    IProductCategoryRepository ProductCategoryRepository { get; }
    IProductRepository ProductRepository { get; }
    IReasonRepository ReasonRepository { get; }
    IStorageRoomRepository StorageRoomRepository { get; }
    ISupplierRepository SupplierRepository { get; }
    IMonthlyStatisticsRepository MonthlyStatisticsRepository { get; }
    IRecipeComponentRepository RecipeComponentRepository { get; }
}