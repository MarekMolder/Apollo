using App.DAL.DTO;
using Base.Contracts;

namespace App.BLL.Mappers;

public class ProductCategoryBLLMapper : IMapper<App.BLL.DTO.ProductCategory, ProductCategory>
{
    private readonly ProductBLLMapper _productBLLMapper = new();
    public ProductCategory? Map(DTO.ProductCategory? entity)
    {
        if (entity == null) return null;
        
        var res = new ProductCategory()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            
            Products = entity.Products?.Select(t => _productBLLMapper.Map(t)).ToList()!,
        };
        return res;
    }

    public DTO.ProductCategory? Map(ProductCategory? entity)
    {
        if (entity == null) return null;
        
        var res = new DTO.ProductCategory()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            
            Products = entity.Products?.Select(t => _productBLLMapper.Map(t)).ToList()!,
        };
        return res;
    }
    
    public static ProductCategory? MapSimple(DTO.ProductCategory? entity)
    {
        if (entity == null) return null;

        return new ProductCategory()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
        };
    }
    
    public static DTO.ProductCategory? MapSimple(ProductCategory? entity)
    {
        if (entity == null) return null;

        return new DTO.ProductCategory()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
        };
    }
}