using App.DAL.DTO;
using Base.Contracts;

namespace App.DAL.EF.Mappers;

public class ProductUOWMapper: IMapper<Product, Domain.Logic.Product>
{
    private readonly ActionEntityUOWMapper _actionEntityUOWMapper = new();
    private readonly CurrentStockUOWMapper _currentStockUOWMapper = new();
    public Product? Map(Domain.Logic.Product? entity)
    {
        if (entity == null) return null;
        
        var res = new Product
        {
            Id = entity.Id,
            Unit = entity.Unit,
            Volume = entity.Volume,
            Code = entity.Code,
            Name = entity.Name,
            Price = entity.Price,
            Quantity = entity.Quantity,
          
            ProductCategoryId = entity.ProductCategoryId,
            ProductCategory = ProductCategoryUOWMapper.MapSimple(entity.ProductCategory),
            
            IsComponent = entity.IsComponent,
            
            Actions = entity.Actions?.Select(t => _actionEntityUOWMapper.Map(t)).ToList()!,
            
            CurrentStocks = entity.CurrentStocks?.Select(t => _currentStockUOWMapper.Map(t)).ToList()!,
        };
        return res;
    }

    public Domain.Logic.Product? Map(Product? entity)
    {
                if (entity == null) return null;
        
        var res = new Domain.Logic.Product
        {
            Id = entity.Id,
            Unit = entity.Unit,
            Volume = entity.Volume,
            Code = entity.Code,
            Name = entity.Name,
            Price = entity.Price,
            Quantity = entity.Quantity,
          
            ProductCategoryId = entity.ProductCategoryId,
            ProductCategory = ProductCategoryUOWMapper.MapSimple(entity.ProductCategory),
            
            IsComponent = entity.IsComponent,
            
            Actions = entity.Actions?.Select(t => _actionEntityUOWMapper.Map(t)).ToList()!,
            
            CurrentStocks = entity.CurrentStocks?.Select(t => _currentStockUOWMapper.Map(t)).ToList()!,
        };
        return res;
    }
    public static Product? MapSimple(Domain.Logic.Product? entity)
    {
        if (entity == null) return null;

        return new Product
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

    public static Domain.Logic.Product? MapSimple(Product? entity)
    {
        if (entity == null) return null;

        return new Domain.Logic.Product
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
