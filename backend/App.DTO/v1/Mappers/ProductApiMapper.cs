using Base.Contracts;

namespace App.DTO.v1.Mappers;

/// <summary>
/// Maps between DTO V1 and BLL DTO Product objects.
/// Includes both full and simplified mapping logic.
/// </summary>
public class ProductApiMapper : IMapper<Product, BLL.DTO.Product>
{
    /// <summary>
    /// Maps a simplified BLL DTO Product to DTO V1 Product, without related objects.
    /// </summary>
    public Product? Map(BLL.DTO.Product? entity)
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
            IsComponent = entity.IsComponent,
        };
        return res;
    }

    /// <summary>
    /// Maps a simplified DTO V1 Product to BLL DTO Product, without related objects.
    /// </summary>
    public BLL.DTO.Product? Map(Product? entity)
    {
        if (entity == null) return null;
        var res = new BLL.DTO.Product
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
        return res;
    }
    
    /// <summary>
    /// Maps a simplified DTO V1 ProductCreate to BLL DTO Product, without related objects.
    /// </summary>
    public BLL.DTO.Product Map(ProductCreate entity)
    {
        var res = new BLL.DTO.Product
        {
            Id = Guid.NewGuid(),
            Unit = entity.Unit,
            Volume = entity.Volume,
            Code = entity.Code,
            Name = entity.Name,
            Price = entity.Price,
            Quantity = entity.Quantity,
            ProductCategoryId = entity.ProductCategoryId,
            IsComponent = entity.IsComponent,
        };
        return res;
    }
}