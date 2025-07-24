using App.DTO.v1.Enums;
using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class ActionTypeEntityAPIMapper : IMapper<ActionTypeEntity, BLL.DTO.ActionTypeEntity>
{
    public ActionTypeEntity? Map(BLL.DTO.ActionTypeEntity? entity)
    {
        if (entity == null) return null;
        var res = new ActionTypeEntity
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (ActionTypeEnum)(int)entity.Code
        };
        return res;
    }

    public BLL.DTO.ActionTypeEntity? Map(ActionTypeEntity? entity)
    {
        if (entity == null) return null;
        var res = new BLL.DTO.ActionTypeEntity
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (BLL.DTO.Enums.ActionTypeEnum)(int)entity.Code
        };
        return res;
    }
    
    public BLL.DTO.ActionTypeEntity Map(ActionTypeEntityCreate entity)
    {
        var res = new BLL.DTO.ActionTypeEntity
        {
            Id = Guid.NewGuid(),
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (BLL.DTO.Enums.ActionTypeEnum)(int)entity.Code
        };
        return res;
    }
}