using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class ReasonAPIMapper : IMapper<Reason, BLL.DTO.Reason>
{
    public Reason? Map(BLL.DTO.Reason? entity)
    {
        if (entity == null) return null;
        var res = new Reason
        {
            Id = entity.Id,
            Description = entity.Description,
            EndedAt = entity.EndedAt,
        };
        return res;
    }

    public BLL.DTO.Reason? Map(Reason? entity)
    {
        if (entity == null) return null;
        var res = new BLL.DTO.Reason
        {
            Id = entity.Id,
            Description = entity.Description,
            EndedAt = entity.EndedAt,
        };
        return res;
    }
    
    public BLL.DTO.Reason Map(ReasonCreate entity)
    {
        var res = new BLL.DTO.Reason
        {
            Id = Guid.NewGuid(),
            Description = entity.Description,
            EndedAt = entity.EndedAt,
        };
        return res;
    }
}