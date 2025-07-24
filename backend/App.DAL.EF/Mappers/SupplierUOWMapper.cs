using App.DAL.DTO;
using Base.Contracts;

namespace App.DAL.EF.Mappers;

public class SupplierUOWMapper: IMapper<Supplier, Domain.Logic.Supplier>
{
    private readonly ActionEntityUOWMapper _actionEntityUOWMapper = new();
    public Supplier? Map(Domain.Logic.Supplier? entity)
    {
        if (entity == null) return null;
        
        var res = new Supplier
        {
            Id = entity.Id,
            Name = entity.Name,
            TelephoneNr = entity.TelephoneNr,
            Email = entity.Email,
            
            AddressId = entity.AddressId,
            Address = AddressUOWMapper.MapSimple(entity.Address),
            
            Actions = entity.Actions?.Select(t => _actionEntityUOWMapper.Map(t)).ToList()!
            
        };
        return res;
    }

    public Domain.Logic.Supplier? Map(Supplier? entity)
    {
        if (entity == null) return null;
        
        var res = new Domain.Logic.Supplier
        {
            Id = entity.Id,
            Name = entity.Name,
            TelephoneNr = entity.TelephoneNr,
            Email = entity.Email,
            
            AddressId = entity.AddressId,
            Address = AddressUOWMapper.MapSimple(entity.Address),
            
            Actions = entity.Actions?.Select(t => _actionEntityUOWMapper.Map(t)).ToList()!
            
        };
        return res;
    }
    
    public static Supplier? MapSimple(Domain.Logic.Supplier? entity)
    {
        if (entity == null) return null;

        return new Supplier
        {
            Id = entity.Id,
            Name = entity.Name,
            TelephoneNr = entity.TelephoneNr,
            Email = entity.Email,
            AddressId = entity.AddressId,
        };
    }

    public static Domain.Logic.Supplier? MapSimple(Supplier? entity)
    {
        if (entity == null) return null;

        return new Domain.Logic.Supplier
        {
            Id = entity.Id,
            Name = entity.Name,
            TelephoneNr = entity.TelephoneNr,
            Email = entity.Email,
            AddressId = entity.AddressId,
        };
    }
}
