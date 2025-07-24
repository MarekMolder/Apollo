using App.DAL.DTO;
using Base.Contracts;

namespace App.DAL.EF.Mappers;

public class ProductCategoryUOWMapper: IMapper<ProductCategory, Domain.Logic.ProductCategory>
{
    private readonly ProductUOWMapper _productUOWMapper = new();
    public ProductCategory? Map(Domain.Logic.ProductCategory? entity)
    {
        if (entity == null) return null;
        
        var res = new ProductCategory
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            
            Products = entity.Products?.Select(t => _productUOWMapper.Map(t)).ToList()!,
           

        };
        return res;
    }

    public Domain.Logic.ProductCategory? Map(ProductCategory? entity)
    {
        if (entity == null) return null;
        
        var res = new Domain.Logic.ProductCategory
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            
            Products = entity.Products?.Select(t => _productUOWMapper.Map(t)).ToList()!,
        };
        return res;
    }
    public static ProductCategory? MapSimple(Domain.Logic.ProductCategory? entity)
    {
        if (entity == null) return null;

        return new ProductCategory
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
        };
    }

    public static Domain.Logic.ProductCategory? MapSimple(ProductCategory? entity)
    {
        if (entity == null) return null;

        return new Domain.Logic.ProductCategory
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
        };
    }
}
