using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class ProductCategoryAPIMapper : IMapper<App.DTO.v1.ProductCategory, App.BLL.DTO.ProductCategory>
{
    public App.DTO.v1.ProductCategory? Map(App.BLL.DTO.ProductCategory? entity)
    {
        if (entity == null) return null;
        var res = new App.DTO.v1.ProductCategory()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
        };
        return res;
    }

    public App.BLL.DTO.ProductCategory? Map(App.DTO.v1.ProductCategory? entity)
    {
        if (entity == null) return null;
        var res = new App.BLL.DTO.ProductCategory()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
        };
        return res;
    }
    
    public App.BLL.DTO.ProductCategory Map(App.DTO.v1.ProductCategoryCreate entity)
    {
        var res = new App.BLL.DTO.ProductCategory()
        {
            Id = Guid.NewGuid(),
            Name = entity.Name,
            EndedAt = entity.EndedAt,
        };
        return res;
    }
}