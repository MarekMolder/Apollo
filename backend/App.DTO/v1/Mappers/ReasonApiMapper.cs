using Base.Contracts;

namespace App.DTO.v1.Mappers;

/// <summary>
/// Maps between DTO V1 and BLL DTO Reason objects.
/// Includes both full and simplified mapping logic.
/// </summary>
public class ReasonApiMapper : IMapper<Reason, BLL.DTO.Reason>
{
    /// <summary>
    /// Maps a simplified BLL DTO Reason to DTO V1 Reason, without related objects.
    /// </summary>
    public Reason? Map(BLL.DTO.Reason? entity)
    {
        if (entity == null) return null;
        var res = new Reason
        {
            Id = entity.Id,
            Description = entity.Description,
        };
        return res;
    }

    /// <summary>
    /// Maps a simplified DTO V1 Reason to BLL DTO Reason, without related objects.
    /// </summary>
    public BLL.DTO.Reason? Map(Reason? entity)
    {
        if (entity == null) return null;
        var res = new BLL.DTO.Reason
        {
            Id = entity.Id,
            Description = entity.Description,
        };
        return res;
    }
    
    /// <summary>
    /// Maps a simplified DTO V1 ReasonCreate to BLL DTO Reason, without related objects.
    /// </summary>
    public BLL.DTO.Reason Map(ReasonCreate entity)
    {
        var res = new BLL.DTO.Reason
        {
            Id = Guid.NewGuid(),
            Description = entity.Description,
        };
        return res;
    }
}