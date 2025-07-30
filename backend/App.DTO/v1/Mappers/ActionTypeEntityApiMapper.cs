using App.DTO.v1.Enums;
using Base.Contracts;

namespace App.DTO.v1.Mappers;

/// <summary>
/// Maps between DTO V1 and BLL DTO ActionTypeEntity objects.
/// Includes both full and simplified mapping logic.
/// </summary>
public class ActionTypeEntityApiMapper : IMapper<ActionTypeEntity, BLL.DTO.ActionTypeEntity>
{
    /// <summary>
    /// Maps a simplified BLL DTO ActionTypeEntity to DTO V1 ActionTypeEntity, without related objects.
    /// </summary>
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

    /// <summary>
    /// Maps a simplified DTO V1 ActionTypeEntity to BLL DTO ActionTypeEntity, without related objects.
    /// </summary>
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
    
    /// <summary>
    /// Maps a simplified DTO V1 ActionTypeEntityCreate to BLL DTO ActionTypeEntity, without related objects.
    /// </summary>
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