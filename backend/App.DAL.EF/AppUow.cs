using App.DAL.Contracts;
using App.DAL.EF.Repositories;
using Base.DAL.EF;

namespace App.DAL.EF;

/// <summary>
/// Central Data Access Layer (DAL) Unit of Work implementation that provides access to all repositories.
/// Repositories are lazily instantiated and operate on a shared database context.
/// </summary>
public class AppUow : BaseUow<AppDbContext>, IAppUOW
{
    public AppUow(AppDbContext uowDbContext) : base(uowDbContext)
    {
        
    }
    
    //ActionEntity Repository
    private IActionEntityRepository? _actionEntityRepository;
    public IActionEntityRepository ActionEntityRepository => 
        _actionEntityRepository ??= new ActionEntityRepository(UowDbContext);
    
    //ActionTypeEntity Repository
    private IActionTypeEntityRepository? _actionTypeEntityRepository;
    public  IActionTypeEntityRepository ActionTypeEntityRepository => 
        _actionTypeEntityRepository ??= new ActionTypeEntityRepository(UowDbContext);
    
    //Address Repository
    private IAddressRepository? _addressRepository;
    public  IAddressRepository AddressRepository => 
        _addressRepository ??= new AddressRepository(UowDbContext);
    
    //ProductCategory Repository
    private IProductCategoryRepository? _productCategoryRepository;
    public  IProductCategoryRepository ProductCategoryRepository => 
        _productCategoryRepository ??= new ProductCategoryRepository(UowDbContext);
    
    //Product Repository
    private IProductRepository? _productRepository;
    public  IProductRepository ProductRepository => 
        _productRepository ??= new ProductRepository(UowDbContext);
    
    //Reason Repository
    private IReasonRepository? _reasonRepository;
    public  IReasonRepository ReasonRepository => 
        _reasonRepository ??= new ReasonRepository(UowDbContext);
    
    //StorageRoom Repository
    private IStorageRoomRepository? _storageRoomRepository;
    public  IStorageRoomRepository StorageRoomRepository => 
        _storageRoomRepository ??= new StorageRoomRepository(UowDbContext);
    
    //Supplier Repository
    private ISupplierRepository? _supplierRepository;
    public  ISupplierRepository SupplierRepository => 
        _supplierRepository ??= new SupplierRepository(UowDbContext);
    
    //MonthlyStatistics Repository
    private IMonthlyStatisticsRepository? _monthlyStatisticsRepository;
    public  IMonthlyStatisticsRepository MonthlyStatisticsRepository => 
        _monthlyStatisticsRepository ??= new MonthlyStatisticsRepository(UowDbContext);
    
    //RecipeComponent Repository
    private IRecipeComponentRepository? _recipeComponentRepository;
    public  IRecipeComponentRepository RecipeComponentRepository => 
        _recipeComponentRepository ??= new RecipeComponentRepository(UowDbContext);
}