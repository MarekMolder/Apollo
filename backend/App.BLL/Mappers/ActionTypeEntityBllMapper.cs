using Base.Contracts;

namespace App.BLL.Mappers;

/// <summary>
/// Maps between BLL DTO and DAL DTO ActionTypeEntity objects.
/// Handles both full mapping and simplified mapping.
/// </summary>
public class ActionTypeEntityBllMapper : IMapper<BLL.DTO.ActionTypeEntity, DAL.DTO.ActionTypeEntity>
{
    // Mapper used to map related ActionEntity objects
    private readonly ActionEntityBllMapper _actionEntityBllMapper = new();
    
    /// <summary>
    /// Maps a full BLL DTO ActionTypeEntity to a DAL DTO ActionTypeEntity, including related Actions.
    /// </summary>
    public DAL.DTO.ActionTypeEntity? Map(BLL.DTO.ActionTypeEntity? entity)
    {
        if (entity == null) return null;
        
        var res = new DAL.DTO.ActionTypeEntity()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (App.DAL.DTO.Enums.ActionTypeEnum)(int)entity.Code,
            Actions = entity.Actions?.Select(t => _actionEntityBllMapper.Map(t)).ToList()!
        };
        return res;
    }

    /// <summary>
    /// Maps a full DAL DTO ActionTypeEntity to a BLL DTO ActionTypeEntity, including related Actions.
    /// </summary>
    public BLL.DTO.ActionTypeEntity? Map(DAL.DTO.ActionTypeEntity? entity)
    {
        if (entity == null) return null;
        
        var res = new BLL.DTO.ActionTypeEntity()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (DTO.Enums.ActionTypeEnum)(int)entity.Code,
            Actions = entity.Actions?.Select(t => _actionEntityBllMapper.Map(t)).ToList()!
        };
        return res;
    }
    
    /// <summary>
    /// Maps a simplified BLL DTO ActionTypeEntity to a DAL DTO ActionTypeEntity (no related Actions).
    /// </summary>
    public static DAL.DTO.ActionTypeEntity? MapSimple(BLL.DTO.ActionTypeEntity? entity)
    {
        if (entity == null) return null;

        return new DAL.DTO.ActionTypeEntity()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            Code = (App.DAL.DTO.Enums.ActionTypeEnum)(int)entity.Code
        };
    }
    
    /// <summary>
    /// Maps a simplified DAL DTO ActionTypeEntity to a BLL DTO ActionTypeEntity (no related Actions).
    /// </summary>
    public static BLL.DTO.ActionTypeEntity? MapSimple(DAL.DTO.ActionTypeEntity? entity)
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
