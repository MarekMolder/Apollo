using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class ProductCategoryAPIMapper : IMapper<ProductCategory, BLL.DTO.ProductCategory>
{
    public ProductCategory? Map(BLL.DTO.ProductCategory? entity)
    {
        if (entity == null) return null;
        var res = new ProductCategory
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
        };
        return res;
    }

    public BLL.DTO.ProductCategory? Map(ProductCategory? entity)
    {
        if (entity == null) return null;
        var res = new BLL.DTO.ProductCategory
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
        };
        return res;
    }
    
    public BLL.DTO.ProductCategory Map(ProductCategoryCreate entity)
    {
        var res = new BLL.DTO.ProductCategory
        {
            Id = Guid.NewGuid(),
            Name = entity.Name,
            EndedAt = entity.EndedAt,
        };
        return res;
    }
}