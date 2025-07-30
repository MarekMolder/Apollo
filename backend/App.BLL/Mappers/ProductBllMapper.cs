using Base.Contracts;

namespace App.BLL.Mappers;

/// <summary>
/// Maps between BLL DTO and DAL DTO Product entities.
/// Handles both full mapping and simplified mapping.
/// </summary>
public class ProductBllMapper : IMapper<BLL.DTO.Product, DAL.DTO.Product>
{
    // Mapper used to map related ActionEntity objects
    private readonly ActionEntityBllMapper _actionEntityBllMapper = new();
    
    /// <summary>
    /// Maps a full BLL DTO Product entity to a DAL DTO Product entity, including related ProductCategories and Actions.
    /// </summary>
    public DAL.DTO.Product? Map(BLL.DTO.Product? entity)
    {
        if (entity == null) return null;
        
        var res = new DAL.DTO.Product()
        {
            Id = entity.Id,
            Unit = entity.Unit,
            Volume = entity.Volume,
            Code = entity.Code,
            Name = entity.Name,
            Price = entity.Price,
            Quantity = entity.Quantity,
          
            ProductCategoryId = entity.ProductCategoryId,
            ProductCategory = ProductCategoryBllMapper.MapSimple(entity.ProductCategory),
            
            IsComponent = entity.IsComponent,
            
            Actions = entity.Actions?.Select(t => _actionEntityBllMapper.Map(t)).ToList()!,
        };
        return res;
    }

    /// <summary>
    /// Maps a full DAL DTO Product entity to a BLL DTO Product entity, including related ProductCategories and Actions.
    /// </summary>
    public BLL.DTO.Product? Map(DAL.DTO.Product? entity)
    {
       if (entity == null) return null;
        
        var res = new BLL.DTO.Product()
        {
            Id = entity.Id,
            Unit = entity.Unit,
            Volume = entity.Volume,
            Code = entity.Code,
            Name = entity.Name,
            Price = entity.Price,
            Quantity = entity.Quantity,
          
            ProductCategoryId = entity.ProductCategoryId,
            ProductCategory = ProductCategoryBllMapper.MapSimple(entity.ProductCategory),
            
            IsComponent = entity.IsComponent,
            
            Actions = entity.Actions?.Select(t => _actionEntityBllMapper.Map(t)).ToList()!,
        };
        return res;
    }
    
    /// <summary>
    /// Maps a simplified BLL DTO Product entity to a DAL DTO Product entity (no related ProductCategories and Actions).
    /// </summary>
    public static DAL.DTO.Product? MapSimple(BLL.DTO.Product? entity)
    {
        if (entity == null) return null;

        return new DAL.DTO.Product()
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
    
    /// <summary>
    /// Maps a simplified DAL DTO Product entity to a BLL DTO Product entity (no related ProductCategories and Actions).
    /// </summary>
    public static BLL.DTO.Product? MapSimple(DAL.DTO.Product? entity)
    {
        if (entity == null) return null;

        return new BLL.DTO.Product()
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
