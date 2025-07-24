using App.DAL.DTO;
using App.DAL.DTO.Enums;
using Base.Contracts;

namespace App.DAL.EF.Mappers;

public class ActionTypeEntityUOWMapper: IMapper<ActionTypeEntity, Domain.Logic.ActionTypeEntity>
{
    private readonly ActionEntityUOWMapper _actionEntityUOWMapper = new();
    public ActionTypeEntity? Map(Domain.Logic.ActionTypeEntity? entity)
    {
        if (entity == null) return null;
        
        var res = new ActionTypeEntity
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (ActionTypeEnum)(int)entity.Code,
            Actions = entity.Actions?.Select(t => _actionEntityUOWMapper.Map(t)).ToList()!
        };
        return res;
    }

    public Domain.Logic.ActionTypeEntity? Map(ActionTypeEntity? entity)
    {
        if (entity == null) return null;
        
        var res = new Domain.Logic.ActionTypeEntity
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (Domain.Enums.ActionTypeEnum)(int)entity.Code,
            Actions = entity.Actions?.Select(t => _actionEntityUOWMapper.Map(t)).ToList()!
        };
        return res;
    }
    
    public static ActionTypeEntity? MapSimple(Domain.Logic.ActionTypeEntity? entity)
    {
        if (entity == null) return null;

        return new ActionTypeEntity
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (ActionTypeEnum)(int)entity.Code
        };
    }

    public static Domain.Logic.ActionTypeEntity? MapSimple(ActionTypeEntity? entity)
    {
        if (entity == null) return null;

        return new Domain.Logic.ActionTypeEntity
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (Domain.Enums.ActionTypeEnum)(int)entity.Code
        };
    }
}
