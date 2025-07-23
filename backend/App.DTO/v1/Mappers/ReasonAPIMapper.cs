using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class ReasonAPIMapper : IMapper<App.DTO.v1.Reason, App.BLL.DTO.Reason>
{
    public App.DTO.v1.Reason? Map(App.BLL.DTO.Reason? entity)
    {
        if (entity == null) return null;
        var res = new App.DTO.v1.Reason()
        {
            Id = entity.Id,
            Description = entity.Description,
            EndedAt = entity.EndedAt,
        };
        return res;
    }

    public App.BLL.DTO.Reason? Map(App.DTO.v1.Reason? entity)
    {
        if (entity == null) return null;
        var res = new App.BLL.DTO.Reason()
        {
            Id = entity.Id,
            Description = entity.Description,
            EndedAt = entity.EndedAt,
        };
        return res;
    }
    
    public App.BLL.DTO.Reason Map(App.DTO.v1.ReasonCreate entity)
    {
        var res = new App.BLL.DTO.Reason()
        {
            Id = Guid.NewGuid(),
            Description = entity.Description,
            EndedAt = entity.EndedAt,
        };
        return res;
    }
}