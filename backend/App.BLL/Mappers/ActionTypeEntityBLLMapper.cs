using App.DAL.DTO;
using Base.BLL.Contracts;
using Base.Contracts;
using App.BLL.DTO.Enums;
using App.DAL.DTO.Enums;
using Base.Helpers;

namespace App.BLL.Mappers;

public class ActionTypeEntityBLLMapper : IMapper<App.BLL.DTO.ActionTypeEntity, App.DAL.DTO.ActionTypeEntity>
{
    private readonly ActionEntityBLLMapper _actionEntityBLLMapper = new();
    public App.DAL.DTO.ActionTypeEntity? Map(BLL.DTO.ActionTypeEntity? entity)
    {
        if (entity == null) return null;
        
        var res = new App.DAL.DTO.ActionTypeEntity()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (App.DAL.DTO.Enums.ActionTypeEnum)(int)entity.Code,
            Actions = entity.Actions?.Select(t => _actionEntityBLLMapper.Map(t)).ToList()!
        };
        return res;
    }

    public BLL.DTO.ActionTypeEntity? Map(App.DAL.DTO.ActionTypeEntity? entity)
    {
        if (entity == null) return null;
        
        var res = new BLL.DTO.ActionTypeEntity()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (App.BLL.DTO.Enums.ActionTypeEnum)(int)entity.Code,
            Actions = entity.Actions?.Select(t => _actionEntityBLLMapper.Map(t)).ToList()!
        };
        return res;
    }
    
    public static App.DAL.DTO.ActionTypeEntity? MapSimple(BLL.DTO.ActionTypeEntity? entity)
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
    
    public static DTO.ActionTypeEntity? MapSimple(App.DAL.DTO.ActionTypeEntity? entity)
    {
        if (entity == null) return null;

        return new BLL.DTO.ActionTypeEntity()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (App.BLL.DTO.Enums.ActionTypeEnum)(int)entity.Code
        };
    }
    
}