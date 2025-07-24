using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class PersonMapper: IMapper<Person, BLL.DTO.Person>
{
    public Person? Map(BLL.DTO.Person? entity)
    {
        if (entity == null) return null;
        var res = new Person
        {
            Id = entity.Id,
            PersonName = entity.PersonName
        };
        return res;
    }

    public BLL.DTO.Person? Map(Person? entity)
    {
        if (entity == null) return null;
        var res = new BLL.DTO.Person
        {
            Id = entity.Id,
            PersonName = entity.PersonName
        };
        return res;
    }
    
    public BLL.DTO.Person Map(PersonCreate entity)
    {
        var res = new BLL.DTO.Person
        {
            Id = Guid.NewGuid(),
            PersonName = entity.PersonName
        };
        return res;
    }
}