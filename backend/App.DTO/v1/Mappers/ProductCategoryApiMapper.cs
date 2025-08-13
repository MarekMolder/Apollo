using Base.Contracts;

namespace App.DTO.v1.Mappers;

/// <summary>
/// Maps between DTO V1 and BLL DTO ProductCategory objects.
/// Includes both full and simplified mapping logic.
/// </summary>
public class ProductCategoryApiMapper : IMapper<ProductCategory, BLL.DTO.ProductCategory>
{
    /// <summary>
    /// Maps a simplified BLL DTO ProductCategory to DTO V1 ProductCategory, without related objects.
    /// </summary>
    public ProductCategory? Map(BLL.DTO.ProductCategory? entity)
    {
        if (entity == null) return null;
        var res = new ProductCategory
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        return res;
    }

    /// <summary>
    /// Maps a simplified DTO V1 ProductCategory to BLL DTO ProductCategory, without related objects.
    /// </summary>
    public BLL.DTO.ProductCategory? Map(ProductCategory? entity)
    {
        if (entity == null) return null;
        var res = new BLL.DTO.ProductCategory
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        return res;
    }
    
    /// <summary>
    /// Maps a simplified DTO V1 ProductCategoryCreate to BLL DTO ProductCategory, without related objects.
    /// </summary>
    public BLL.DTO.ProductCategory Map(ProductCategoryCreate entity)
    {
        var res = new BLL.DTO.ProductCategory
        {
            Id = Guid.NewGuid(),
            Name = entity.Name,
        };
        return res;
    }
}