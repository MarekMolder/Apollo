using App.DAL.DTO;
using Base.Contracts;
using Base.DAL.Contracts;

namespace App.DAL.EF.Mappers;

public class ActionTypeEntityUOWMapper: IMapper<App.DAL.DTO.ActionTypeEntity, App.Domain.Logic.ActionTypeEntity>
{
    private readonly ActionEntityUOWMapper _actionEntityUOWMapper = new();
    public App.DAL.DTO.ActionTypeEntity? Map(Domain.Logic.ActionTypeEntity? entity)
    {
        if (entity == null) return null;
        
        var res = new App.DAL.DTO.ActionTypeEntity()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (App.DAL.DTO.Enums.ActionTypeEnum)(int)entity.Code,
            Actions = entity.Actions?.Select(t => _actionEntityUOWMapper.Map(t)).ToList()!
        };
        return res;
    }

    public Domain.Logic.ActionTypeEntity? Map(App.DAL.DTO.ActionTypeEntity? entity)
    {
        if (entity == null) return null;
        
        var res = new Domain.Logic.ActionTypeEntity()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (App.Domain.Enums.ActionTypeEnum)(int)entity.Code,
            Actions = entity.Actions?.Select(t => _actionEntityUOWMapper.Map(t)).ToList()!
        };
        return res;
    }
    
    public static App.DAL.DTO.ActionTypeEntity? MapSimple(Domain.Logic.ActionTypeEntity? entity)
    {
        if (entity == null) return null;

        return new App.DAL.DTO.ActionTypeEntity()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (App.DAL.DTO.Enums.ActionTypeEnum)(int)entity.Code
        };
    }

    public static Domain.Logic.ActionTypeEntity? MapSimple(App.DAL.DTO.ActionTypeEntity? entity)
    {
        if (entity == null) return null;

        return new Domain.Logic.ActionTypeEntity()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (App.Domain.Enums.ActionTypeEnum)(int)entity.Code
        };
    }
}
