using Base.Contracts;

namespace App.DAL.EF.Mappers;

/// <summary>
/// Maps between Domain and DAL DTO Product entities.
/// Handles both full mapping and simplified mapping.
/// </summary>
public class ProductUowMapper: IMapper<DAL.DTO.Product, Domain.Logic.Product>
{
    // Mapper used to map related ActionEntity objects
    private readonly ActionEntityUowMapper _actionEntityUowMapper = new();
    
    /// <summary>
    /// Maps a full Domain Product entity to a DAL DTO Product entity, including related ProductCategories and Actions.
    /// </summary>
    public DAL.DTO.Product? Map(Domain.Logic.Product? entity)
    {
        if (entity == null) return null;
        
        var res = new DAL.DTO.Product
        {
            Id = entity.Id,
            Unit = entity.Unit,
            Volume = entity.Volume,
            Code = entity.Code,
            Name = entity.Name,
            Price = entity.Price,
            Quantity = entity.Quantity,
          
            ProductCategoryId = entity.ProductCategoryId,
            ProductCategory = ProductCategoryUowMapper.MapSimple(entity.ProductCategory),
            
            IsComponent = entity.IsComponent,
            
            SupplierId = entity.SupplierId,
            Supplier = SupplierUowMapper.MapSimple(entity.Supplier),
            
            Actions = entity.Actions?.Select(t => _actionEntityUowMapper.Map(t)).ToList()!,
        };
        return res;
    }

    /// <summary>
    /// Maps a full DAL DTO Product entity to a Domain Product entity, including related ProductCategories and Actions.
    /// </summary>
    public Domain.Logic.Product? Map(DAL.DTO.Product? entity)
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
            ProductCategory = ProductCategoryUowMapper.MapSimple(entity.ProductCategory),
            
            IsComponent = entity.IsComponent,
            
            SupplierId = entity.SupplierId,
            Supplier = SupplierUowMapper.MapSimple(entity.Supplier),
            
            Actions = entity.Actions?.Select(t => _actionEntityUowMapper.Map(t)).ToList()!,
        };
        return res;
    }
    
    /// <summary>
    /// Maps a simplified Domain Product entity to a DAL DTO Product entity (no related ProductCategories and Actions).
    /// </summary>
    public static DAL.DTO.Product? MapSimple(Domain.Logic.Product? entity)
    {
        if (entity == null) return null;

        return new DAL.DTO.Product
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
            SupplierId = entity.SupplierId,
        };
    }

    /// <summary>
    /// Maps a simplified DAL DTO Product entity to a Domain Product entity (no related ProductCategories and Actions).
    /// </summary>
    public static Domain.Logic.Product? MapSimple(DAL.DTO.Product? entity)
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
            SupplierId = entity.SupplierId,
        };
    }
}
