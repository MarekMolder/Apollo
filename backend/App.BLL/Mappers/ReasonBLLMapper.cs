using App.DAL.DTO;
using Base.Contracts;

namespace App.BLL.Mappers;

public class ReasonBLLMapper : IMapper<App.BLL.DTO.Reason, Reason>
{
    private readonly ActionEntityBLLMapper _actionEntityBLLMapper = new();
    public Reason? Map(DTO.Reason? entity)
    {
        if (entity == null) return null;
        
        var res = new Reason()
        {
            Id = entity.Id,
            Description = entity.Description,
            EndedAt = entity.EndedAt,
            
            Actions = entity.Actions?.Select(t => _actionEntityBLLMapper.Map(t)).ToList()!
        };
        return res;
    }

    public DTO.Reason? Map(Reason? entity)
    {
        if (entity == null) return null;
        
        var res = new DTO.Reason()
        {
            Id = entity.Id,
            Description = entity.Description,
            EndedAt = entity.EndedAt,
            
            Actions = entity.Actions?.Select(t => _actionEntityBLLMapper.Map(t)).ToList()!
        };
        return res;
    }
    
    public static Reason? MapSimple(DTO.Reason? entity)
    {
        if (entity == null) return null;

        return new Reason()
        {
            Id = entity.Id,
            Description = entity.Description,
            EndedAt = entity.EndedAt,
        };
    }
    
    public static DTO.Reason? MapSimple(Reason? entity)
    {
        if (entity == null) return null;

        return new DTO.Reason()
        {
            Id = entity.Id,
            Description = entity.Description,
            EndedAt = entity.EndedAt,
        };
    }
}