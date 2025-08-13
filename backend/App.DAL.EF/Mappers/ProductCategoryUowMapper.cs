using Base.Contracts;

namespace App.DAL.EF.Mappers;

/// <summary>
/// Maps between Domain and DAL DTO ProductCategory entities.
/// Handles both full mapping and simplified mapping.
/// </summary>
public class ProductCategoryUowMapper: IMapper<DAL.DTO.ProductCategory, Domain.Logic.ProductCategory>
{
    // Mapper used to map related Product objects
    private readonly ProductUowMapper _productUowMapper = new();
    
    /// <summary>
    /// Maps a full Domain ProductCategory entity to a DAL DTO ProductCategory entity, including related Products.
    /// </summary>
    public DAL.DTO.ProductCategory? Map(Domain.Logic.ProductCategory? entity)
    {
        if (entity == null) return null;
        
        var res = new DAL.DTO.ProductCategory
        {
            Id = entity.Id,
            Name = entity.Name,
            
            Products = entity.Products?.Select(t => _productUowMapper.Map(t)).ToList()!,
        };
        return res;
    }

    /// <summary>
    /// Maps a full DAL DTO ProductCategory entity to a Domain ProductCategory entity, including related Products.
    /// </summary>
    public Domain.Logic.ProductCategory? Map(DAL.DTO.ProductCategory? entity)
    {
        if (entity == null) return null;
        
        var res = new Domain.Logic.ProductCategory
        {
            Id = entity.Id,
            Name = entity.Name,
            
            Products = entity.Products?.Select(t => _productUowMapper.Map(t)).ToList()!,
        };
        return res;
    }
    
    /// <summary>
    /// Maps a simplified Domain ProductCategory entity to a DAL DTO ProductCategory entity (no related Products).
    /// </summary>
    public static DAL.DTO.ProductCategory? MapSimple(Domain.Logic.ProductCategory? entity)
    {
        if (entity == null) return null;

        return new DAL.DTO.ProductCategory
        {
            Id = entity.Id,
            Name = entity.Name,
        };
    }

    /// <summary>
    /// Maps a simplified DAL DTO ProductCategory entity to a Domain ProductCategory entity (no related Products).
    /// </summary>
    public static Domain.Logic.ProductCategory? MapSimple(DAL.DTO.ProductCategory? entity)
    {
        if (entity == null) return null;

        return new Domain.Logic.ProductCategory
        {
            Id = entity.Id,
            Name = entity.Name,
        };
    }
}
