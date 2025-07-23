using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class ActionTypeEntityAPIMapper : IMapper<App.DTO.v1.ActionTypeEntity, App.BLL.DTO.ActionTypeEntity>
{
    public App.DTO.v1.ActionTypeEntity? Map(App.BLL.DTO.ActionTypeEntity? entity)
    {
        if (entity == null) return null;
        var res = new App.DTO.v1.ActionTypeEntity()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (App.DTO.v1.Enums.ActionTypeEnum)(int)entity.Code
        };
        return res;
    }

    public App.BLL.DTO.ActionTypeEntity? Map(App.DTO.v1.ActionTypeEntity? entity)
    {
        if (entity == null) return null;
        var res = new App.BLL.DTO.ActionTypeEntity()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (App.BLL.DTO.Enums.ActionTypeEnum)(int)entity.Code
        };
        return res;
    }
    
    public App.BLL.DTO.ActionTypeEntity Map(App.DTO.v1.ActionTypeEntityCreate entity)
    {
        var res = new App.BLL.DTO.ActionTypeEntity()
        {
            Id = Guid.NewGuid(),
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (App.BLL.DTO.Enums.ActionTypeEnum)(int)entity.Code
        };
        return res;
    }
}