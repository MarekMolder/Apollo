using App.DAL.DTO;
using Base.Contracts;

namespace App.BLL.Mappers;

public class ProductBLLMapper : IMapper<App.BLL.DTO.Product, Product>
{
    private readonly ActionEntityBLLMapper _actionEntityBLLMapper = new();
    private readonly CurrentStockBLLMapper _currentStockBLLMapper = new();
    
    public Product? Map(DTO.Product? entity)
    {
        if (entity == null) return null;
        
        var res = new Product()
        {
            Id = entity.Id,
            Unit = entity.Unit,
            Volume = entity.Volume,
            Code = entity.Code,
            Name = entity.Name,
            Price = entity.Price,
            Quantity = entity.Quantity,
          
            ProductCategoryId = entity.ProductCategoryId,
            ProductCategory = ProductCategoryBLLMapper.MapSimple(entity.ProductCategory),
            
            IsComponent = entity.IsComponent,
            
            Actions = entity.Actions?.Select(t => _actionEntityBLLMapper.Map(t)).ToList()!,
            
            CurrentStocks = entity.CurrentStocks?.Select(t => _currentStockBLLMapper.Map(t)).ToList()!,
        };
        return res;
    }

    public DTO.Product? Map(Product? entity)
    {
       if (entity == null) return null;
        
        var res = new DTO.Product()
        {
            Id = entity.Id,
            Unit = entity.Unit,
            Volume = entity.Volume,
            Code = entity.Code,
            Name = entity.Name,
            Price = entity.Price,
            Quantity = entity.Quantity,
          
            ProductCategoryId = entity.ProductCategoryId,
            ProductCategory = ProductCategoryBLLMapper.MapSimple(entity.ProductCategory),
            
            IsComponent = entity.IsComponent,
            
            Actions = entity.Actions?.Select(t => _actionEntityBLLMapper.Map(t)).ToList()!,
            
            CurrentStocks = entity.CurrentStocks?.Select(t => _currentStockBLLMapper.Map(t)).ToList()!,
        };
        return res;
    }
    
    public static Product? MapSimple(DTO.Product? entity)
    {
        if (entity == null) return null;

        return new Product()
        {
            Id = entity.Id,
            Unit = entity.Unit,
            Volume = entity.Volume,
            Code = entity.Code,
            Name = entity.Name,
            Price = entity.Price,
            Quantity = entity.Quantity,
            ProductCategoryId = entity.ProductCategoryId,
            IsComponent = entity.IsComponent,
        };
    }
    
    public static DTO.Product? MapSimple(Product? entity)
    {
        if (entity == null) return null;

        return new DTO.Product()
        {
            Id = entity.Id,
            Unit = entity.Unit,
            Volume = entity.Volume,
            Code = entity.Code,
            Name = entity.Name,
            Price = entity.Price,
            Quantity = entity.Quantity,
            ProductCategoryId = entity.ProductCategoryId,
            IsComponent = entity.IsComponent,
        };
    }
}