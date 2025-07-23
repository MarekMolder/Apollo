using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class ProductAPIMapper : IMapper<App.DTO.v1.Product, App.BLL.DTO.Product>
{
    public App.DTO.v1.Product? Map(App.BLL.DTO.Product? entity)
    {
        if (entity == null) return null;
        var res = new App.DTO.v1.Product()
        {
            Id = entity.Id,
            Unit = entity.Unit,
            Volume = entity.Volume,
            Code = entity.Code,
            Name = entity.Name,
            Price = entity.Price,
            Quantity = entity.Quantity,
            ProductCategoryId = entity.ProductCategoryId,
        };
        return res;
    }

    public App.BLL.DTO.Product? Map(App.DTO.v1.Product? entity)
    {
        if (entity == null) return null;
        var res = new App.BLL.DTO.Product()
        {
            Id = entity.Id,
            Unit = entity.Unit,
            Volume = entity.Volume,
            Code = entity.Code,
            Name = entity.Name,
            Price = entity.Price,
            Quantity = entity.Quantity,
            ProductCategoryId = entity.ProductCategoryId,
        };
        return res;
    }
    
    public App.BLL.DTO.Product Map(App.DTO.v1.ProductCreate entity)
    {
        var res = new App.BLL.DTO.Product()
        {
            Id = Guid.NewGuid(),
            Unit = entity.Unit,
            Volume = entity.Volume,
            Code = entity.Code,
            Name = entity.Name,
            Price = entity.Price,
            Quantity = entity.Quantity,
            ProductCategoryId = entity.ProductCategoryId,
        };
        return res;
    }
}