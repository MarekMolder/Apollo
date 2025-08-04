using Base.Contracts;

namespace App.BLL.Mappers;

/// <summary>
/// Maps between BLL DTO and DAL DTO ProductCategory entities.
/// Handles both full mapping and simplified mapping.
/// </summary>
public class ProductCategoryBllMapper : IMapper<BLL.DTO.ProductCategory, DAL.DTO.ProductCategory>
{
    // Mapper used to map related Product objects
    private readonly ProductBllMapper _productBllMapper = new();
    
    /// <summary>
    /// Maps a full BLL DTO ProductCategory entity to a DAL DTO ProductCategory entity, including related Products.
    /// </summary>
    public DAL.DTO.ProductCategory? Map(BLL.DTO.ProductCategory? entity)
    {
        if (entity == null) return null;
        
        var res = new DAL.DTO.ProductCategory()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            
            Products = entity.Products?.Select(t => _productBllMapper.Map(t)).ToList()!,
        };
        return res;
    }

    /// <summary>
    /// Maps a full DAL DTO ProductCategory entity to a BLL DTO ProductCategory entity, including related Products.
    /// </summary>
    public BLL.DTO.ProductCategory? Map(DAL.DTO.ProductCategory? entity)
    {
        if (entity == null) return null;
        
        var res = new BLL.DTO.ProductCategory()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            
            Products = entity.Products?.Select(t => _productBllMapper.Map(t)).ToList()!,
        };
        return res;
    }
    
    /// <summary>
    /// Maps a simplified BLL DTO ProductCategory entity to a DAL DTO ProductCategory entity (no related Products).
    /// </summary>
    public static DAL.DTO.ProductCategory? MapSimple(BLL.DTO.ProductCategory? entity)
    {
        if (entity == null) return null;

        return new DAL.DTO.ProductCategory()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
        };
    }
    
    /// <summary>
    /// Maps a simplified DAL DTO ProductCategory entity to a BLL DTO ProductCategory entity (no related Products).
    /// </summary>
    public static BLL.DTO.ProductCategory? MapSimple(DAL.DTO.ProductCategory? entity)
    {
        if (entity == null) return null;

        return new BLL.DTO.ProductCategory()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
        };
    }
}