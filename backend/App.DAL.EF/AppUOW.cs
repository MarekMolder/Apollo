using App.DAL.Contracts;
using App.DAL.EF.Repositories;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUOW : BaseUOW<AppDbContext>, IAppUOW
{

    
    public AppUOW(AppDbContext uowDbContext) : base(uowDbContext)
    {
        
    }
    
    private IActionEntityRepository? _actionEntityRepository;
    public IActionEntityRepository ActionEntityRepository => 
        _actionEntityRepository ??= new ActionEntityRepository(UOWDbContext);
    
    private IActionTypeEntityRepository? _actionTypeEntityRepository;
    public  IActionTypeEntityRepository ActionTypeEntityRepository => 
        _actionTypeEntityRepository ??= new ActionTypeEntityRepository(UOWDbContext);
    
    private IAddressRepository? _addressRepository;
    public  IAddressRepository AddressRepository => 
        _addressRepository ??= new AddressRepository(UOWDbContext);
    
    private ICurrentStockRepository? _currentStockRepository;
    public  ICurrentStockRepository CurrentStockRepository => 
        _currentStockRepository ??= new CurrentStockRepository(UOWDbContext);
    
    private IProductCategoryRepository? _productCategoryRepository;
    public  IProductCategoryRepository ProductCategoryRepository => 
        _productCategoryRepository ??= new ProductCategoryRepository(UOWDbContext);
    
    private IProductRepository? _productRepository;
    public  IProductRepository ProductRepository => 
        _productRepository ??= new ProductRepository(UOWDbContext);
    
    private IReasonRepository? _reasonRepository;
    public  IReasonRepository ReasonRepository => 
        _reasonRepository ??= new ReasonRepository(UOWDbContext);
    
    private IStorageRoomRepository? _storageRoomRepository;
    public  IStorageRoomRepository StorageRoomRepository => 
        _storageRoomRepository ??= new StorageRoomRepository(UOWDbContext);
    
    private ISupplierRepository? _supplierRepository;
    public  ISupplierRepository SupplierRepository => 
        _supplierRepository ??= new SupplierRepository(UOWDbContext);
    
    private IMonthlyStatisticsRepository? _monthlyStatisticsRepository;
    public  IMonthlyStatisticsRepository MonthlyStatisticsRepository => 
        _monthlyStatisticsRepository ??= new MonthlyStatisticsRepository(UOWDbContext);
    
    private IRecipeComponentRepository? _recipeComponentRepository;
    public  IRecipeComponentRepository RecipeComponentRepository => 
        _recipeComponentRepository ??= new RecipeComponentRepositoryRepository(UOWDbContext);
}