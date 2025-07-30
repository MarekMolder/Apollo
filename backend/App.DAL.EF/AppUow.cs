using App.DAL.Contracts;
using App.DAL.EF.Repositories;
using Base.DAL.EF;

namespace App.DAL.EF;

/// <summary>
/// Central Data Access Layer (DAL) Unit of Work implementation that provides access to all repositories.
/// Repositories are lazily instantiated and operate on a shared database context.
/// </summary>
public class AppUow : BaseUOW<AppDbContext>, IAppUOW
{
    public AppUow(AppDbContext uowDbContext) : base(uowDbContext)
    {
        
    }
    
    //ActionEntity Repository
    private IActionEntityRepository? _actionEntityRepository;
    public IActionEntityRepository ActionEntityRepository => 
        _actionEntityRepository ??= new ActionEntityRepository(UOWDbContext);
    
    //ActionTypeEntity Repository
    private IActionTypeEntityRepository? _actionTypeEntityRepository;
    public  IActionTypeEntityRepository ActionTypeEntityRepository => 
        _actionTypeEntityRepository ??= new ActionTypeEntityRepository(UOWDbContext);
    
    //Address Repository
    private IAddressRepository? _addressRepository;
    public  IAddressRepository AddressRepository => 
        _addressRepository ??= new AddressRepository(UOWDbContext);
    
    //ProductCategory Repository
    private IProductCategoryRepository? _productCategoryRepository;
    public  IProductCategoryRepository ProductCategoryRepository => 
        _productCategoryRepository ??= new ProductCategoryRepository(UOWDbContext);
    
    //Product Repository
    private IProductRepository? _productRepository;
    public  IProductRepository ProductRepository => 
        _productRepository ??= new ProductRepository(UOWDbContext);
    
    //Reason Repository
    private IReasonRepository? _reasonRepository;
    public  IReasonRepository ReasonRepository => 
        _reasonRepository ??= new ReasonRepository(UOWDbContext);
    
    //StorageRoom Repository
    private IStorageRoomRepository? _storageRoomRepository;
    public  IStorageRoomRepository StorageRoomRepository => 
        _storageRoomRepository ??= new StorageRoomRepository(UOWDbContext);
    
    //Supplier Repository
    private ISupplierRepository? _supplierRepository;
    public  ISupplierRepository SupplierRepository => 
        _supplierRepository ??= new SupplierRepository(UOWDbContext);
    
    //MonthlyStatistics Repository
    private IMonthlyStatisticsRepository? _monthlyStatisticsRepository;
    public  IMonthlyStatisticsRepository MonthlyStatisticsRepository => 
        _monthlyStatisticsRepository ??= new MonthlyStatisticsRepository(UOWDbContext);
    
    //RecipeComponent Repository
    private IRecipeComponentRepository? _recipeComponentRepository;
    public  IRecipeComponentRepository RecipeComponentRepository => 
        _recipeComponentRepository ??= new RecipeComponentRepository(UOWDbContext);
}