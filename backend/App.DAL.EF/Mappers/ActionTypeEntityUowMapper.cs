using App.DAL.DTO.Enums;
using Base.Contracts;

namespace App.DAL.EF.Mappers;

/// <summary>
/// Maps between Domain and DAL DTO ActionTypeEntity objects.
/// Handles both full mapping and simplified mapping.
/// </summary>
public class ActionTypeEntityUowMapper: IMapper<DAL.DTO.ActionTypeEntity, Domain.Logic.ActionTypeEntity>
{
    // Mapper used to map related ActionEntity objects
    private readonly ActionEntityUowMapper _actionEntityUowMapper = new();
    
    /// <summary>
    /// Maps a full Domain ActionTypeEntity to a DAL DTO ActionTypeEntity, including related Actions.
    /// </summary>
    public DAL.DTO.ActionTypeEntity? Map(Domain.Logic.ActionTypeEntity? entity)
    {
        if (entity == null) return null;
        
        var res = new DAL.DTO.ActionTypeEntity
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (ActionTypeEnum)(int)entity.Code,
            Actions = entity.Actions?.Select(t => _actionEntityUowMapper.Map(t)).ToList()!
        };
        return res;
    }

    /// <summary>
    /// Maps a full DAL DTO ActionTypeEntity to a Domain ActionTypeEntity, including related Actions.
    /// </summary>
    public Domain.Logic.ActionTypeEntity? Map(DAL.DTO.ActionTypeEntity? entity)
    {
        if (entity == null) return null;
        
        var res = new Domain.Logic.ActionTypeEntity
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (Domain.Enums.ActionTypeEnum)(int)entity.Code,
            Actions = entity.Actions?.Select(t => _actionEntityUowMapper.Map(t)).ToList()!
        };
        return res;
    }
    
    /// <summary>
    /// Maps a simplified Domain ActionTypeEntity to a DAL DTO ActionTypeEntity (no related Actions).
    /// </summary>
    public static DAL.DTO.ActionTypeEntity? MapSimple(Domain.Logic.ActionTypeEntity? entity)
    {
        if (entity == null) return null;

        return new DAL.DTO.ActionTypeEntity
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (ActionTypeEnum)(int)entity.Code
        };
    }

    /// <summary>
    /// Maps a simplified DAL DTO ActionTypeEntity to a Domain ActionTypeEntity (no related Actions).
    /// </summary>
    public static Domain.Logic.ActionTypeEntity? MapSimple(DAL.DTO.ActionTypeEntity? entity)
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
