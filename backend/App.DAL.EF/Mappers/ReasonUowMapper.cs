using Base.Contracts;

namespace App.DAL.EF.Mappers;

/// <summary>
/// Maps between Domain and DAL DTO Reason entities.
/// Handles both full mapping and simplified mapping.
/// </summary>
public class ReasonUowMapper: IMapper<DAL.DTO.Reason, Domain.Logic.Reason>
{
    // Mapper used to map related ActionEntity objects
    private readonly ActionEntityUowMapper _actionEntityUowMapper = new();
    
    /// <summary>
    /// Maps a full Domain Reason entity to a DAL DTO Reason entity, including related ActionEntities.
    /// </summary>
    public DAL.DTO.Reason? Map(Domain.Logic.Reason? entity)
    {
        if (entity == null) return null;
        
        var res = new DAL.DTO.Reason
        {
            Id = entity.Id,
            Description = entity.Description,
            
            Actions = entity.Actions?.Select(t => _actionEntityUowMapper.Map(t)).ToList()!
        };
        return res;
    }

    /// <summary>
    /// Maps a full DAL DTO Reason entity to a Domain Reason entity, including related ActionEntities.
    /// </summary>
    public Domain.Logic.Reason? Map(DAL.DTO.Reason? entity)
    {
        if (entity == null) return null;
        
        var res = new Domain.Logic.Reason
        {
            Id = entity.Id,
            Description = entity.Description,
            
            Actions = entity.Actions?.Select(t => _actionEntityUowMapper.Map(t)).ToList()!
        };
        return res;
    }
    
    /// <summary>
    /// Maps a simplified Domain Reason entity to a DAL DTO Reason entity (no related ActionEntities).
    /// </summary>
    public static DAL.DTO.Reason? MapSimple(Domain.Logic.Reason? entity)
    {
        if (entity == null) return null;

        return new DAL.DTO.Reason
        {
            Id = entity.Id,
            Description = entity.Description,
        };
    }

    /// <summary>
    /// Maps a simplified DAL DTO Reason entity to a Domain Reason entity (no related ActionEntities).
    /// </summary>
    public static Domain.Logic.Reason? MapSimple(DAL.DTO.Reason? entity)
    {
        if (entity == null) return null;

        return new Domain.Logic.Reason
        {
            Id = entity.Id,
            Description = entity.Description,
        };
    }
}