using Base.Contracts;

namespace App.BLL.Mappers;

/// <summary>
/// Maps between BLL DTO and DAL DTO Reason entities.
/// Handles both full mapping and simplified mapping.
/// </summary>
public class ReasonBllMapper : IMapper<BLL.DTO.Reason, DAL.DTO.Reason>
{
    // Mapper used to map related ActionEntity objects
    private readonly ActionEntityBllMapper _actionEntityBllMapper = new();
    
    /// <summary>
    /// Maps a full BLL DTO Reason entity to a DAL DTO Reason entity, including related ActionEntities.
    /// </summary>
    public DAL.DTO.Reason? Map(BLL.DTO.Reason? entity)
    {
        if (entity == null) return null;
        
        var res = new DAL.DTO.Reason()
        {
            Id = entity.Id,
            Description = entity.Description,
            EndedAt = entity.EndedAt,
            
            Actions = entity.Actions?.Select(t => _actionEntityBllMapper.Map(t)).ToList()!
        };
        return res;
    }

    /// <summary>
    /// Maps a full DAL DTO Reason entity to a BLL DTO Reason entity, including related ActionEntities.
    /// </summary>
    public BLL.DTO.Reason? Map(DAL.DTO.Reason? entity)
    {
        if (entity == null) return null;
        
        var res = new BLL.DTO.Reason()
        {
            Id = entity.Id,
            Description = entity.Description,
            EndedAt = entity.EndedAt,
            
            Actions = entity.Actions?.Select(t => _actionEntityBllMapper.Map(t)).ToList()!
        };
        return res;
    }
    
    /// <summary>
    /// Maps a simplified BLL DTO Reason entity to a DAL DTO Reason entity (no related ActionEntities).
    /// </summary>
    public static DAL.DTO.Reason? MapSimple(BLL.DTO.Reason? entity)
    {
        if (entity == null) return null;

        return new DAL.DTO.Reason()
        {
            Id = entity.Id,
            Description = entity.Description,
            EndedAt = entity.EndedAt,
        };
    }
    
    /// <summary>
    /// Maps a simplified DAL DTO Reason entity to a BLL DTO Reason entity (no related ActionEntities).
    /// </summary>
    public static BLL.DTO.Reason? MapSimple(DAL.DTO.Reason? entity)
    {
        if (entity == null) return null;

        return new BLL.DTO.Reason()
        {
            Id = entity.Id,
            Description = entity.Description,
            EndedAt = entity.EndedAt,
        };
    }
}