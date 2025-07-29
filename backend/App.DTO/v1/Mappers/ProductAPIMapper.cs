using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class ProductAPIMapper : IMapper<Product, BLL.DTO.Product>
{
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