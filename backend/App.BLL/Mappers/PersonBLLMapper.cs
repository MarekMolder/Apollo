using App.DAL.DTO;
using Base.BLL.Contracts;
using Base.Contracts;

namespace App.BLL.Mappers;

public class PersonBLLMapper : IMapper<App.BLL.DTO.Person, App.DAL.DTO.Person>
{
    private readonly ActionEntityBLLMapper _actionEntityBLLMapper = new();
    
    public Person? Map(DTO.Person? entity)
    {
        if (entity == null) return null;
        
        var res = new Person()
        {
            Id = entity.Id,
            PersonName = entity.PersonName,
            Actions = entity.Actions?.Select(t => _actionEntityBLLMapper.Map(t)).ToList()!,
        };
        return res;    
    }

    public DTO.Person? Map(Person? entity)
    {
        if (entity == null) return null;
        var res = new DTO.Person()
        {
            Id = entity.Id,
            PersonName = entity.PersonName,
            Actions = entity.Actions?.Select(t => _actionEntityBLLMapper.Map(t)).ToList()!,
        };
        return res;
        
    }
    
    public static Person? MapSimple(DTO.Person? entity)
    {
        if (entity == null) return null;

        return new Person()
        {
            Id = entity.Id,
            PersonName = entity.PersonName,
        };
    }
    
    public static DTO.Person? MapSimple(Person? entity)
    {
        if (entity == null) return null;

        return new DTO.Person()
        {
            Id = entity.Id,
            PersonName = entity.PersonName,
        };
    }
}