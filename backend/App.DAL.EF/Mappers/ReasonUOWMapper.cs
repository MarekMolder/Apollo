using App.DAL.DTO;
using Base.Contracts;
using Base.DAL.Contracts;

namespace App.DAL.EF.Mappers;

public class ReasonUOWMapper: IMapper<App.DAL.DTO.Reason, App.Domain.Logic.Reason>
{
    private readonly ActionEntityUOWMapper _actionEntityUOWMapper = new();
    public Reason? Map(Domain.Logic.Reason? entity)
    {
        if (entity == null) return null;
        
        var res = new Reason()
        {
            Id = entity.Id,
            Description = entity.Description,
            EndedAt = entity.EndedAt,
            
            Actions = entity.Actions?.Select(t => _actionEntityUOWMapper.Map(t)).ToList()!
        };
        return res;
    }

    public Domain.Logic.Reason? Map(Reason? entity)
    {
        if (entity == null) return null;
        
        var res = new Domain.Logic.Reason()
        {
            Id = entity.Id,
            Description = entity.Description,
            EndedAt = entity.EndedAt,
            
            Actions = entity.Actions?.Select(t => _actionEntityUOWMapper.Map(t)).ToList()!
        };
        return res;
    }
    
    public static Reason? MapSimple(Domain.Logic.Reason? entity)
    {
        if (entity == null) return null;

        return new Reason()
        {
            Id = entity.Id,
            Description = entity.Description,
            EndedAt = entity.EndedAt,
        };
    }

    public static Domain.Logic.Reason? MapSimple(Reason? entity)
    {
        if (entity == null) return null;

        return new Domain.Logic.Reason()
        {
            Id = entity.Id,
            Description = entity.Description,
            EndedAt = entity.EndedAt,
        };
    }
}