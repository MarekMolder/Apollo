using App.DAL.DTO;
using Base.Contracts;

namespace App.BLL.Mappers;

public class ActionTypeEntityBLLMapper : IMapper<App.BLL.DTO.ActionTypeEntity, ActionTypeEntity>
{
    private readonly ActionEntityBLLMapper _actionEntityBLLMapper = new();
    public ActionTypeEntity? Map(BLL.DTO.ActionTypeEntity? entity)
    {
        if (entity == null) return null;
        
        var res = new ActionTypeEntity()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (App.DAL.DTO.Enums.ActionTypeEnum)(int)entity.Code,
            Actions = entity.Actions?.Select(t => _actionEntityBLLMapper.Map(t)).ToList()!
        };
        return res;
    }

    public BLL.DTO.ActionTypeEntity? Map(ActionTypeEntity? entity)
    {
        if (entity == null) return null;
        
        var res = new BLL.DTO.ActionTypeEntity()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (DTO.Enums.ActionTypeEnum)(int)entity.Code,
            Actions = entity.Actions?.Select(t => _actionEntityBLLMapper.Map(t)).ToList()!
        };
        return res;
    }
    
    public static ActionTypeEntity? MapSimple(BLL.DTO.ActionTypeEntity? entity)
    {
        if (entity == null) return null;

        return new ActionTypeEntity()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (App.DAL.DTO.Enums.ActionTypeEnum)(int)entity.Code
        };
    }
    
    public static DTO.ActionTypeEntity? MapSimple(ActionTypeEntity? entity)
    {
        if (entity == null) return null;

        return new BLL.DTO.ActionTypeEntity()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (DTO.Enums.ActionTypeEnum)(int)entity.Code
        };
    }
    
}