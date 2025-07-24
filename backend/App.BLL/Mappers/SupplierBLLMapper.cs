using App.DAL.DTO;
using Base.Contracts;

namespace App.BLL.Mappers;

public class SupplierBLLMapper : IMapper<App.BLL.DTO.Supplier, App.DAL.DTO.Supplier>
{
    private readonly ActionEntityBLLMapper _actionEntityBLLMapper = new();
    public Supplier? Map(DTO.Supplier? entity)
    {
        if (entity == null) return null;
        
        var res = new Supplier()
        {
            Id = entity.Id,
            Name = entity.Name,
            TelephoneNr = entity.TelephoneNr,
            Email = entity.Email,
            
            AddressId = entity.AddressId,
            Address = AddressBLLMapper.MapSimple(entity.Address),
            
            Actions = entity.Actions?.Select(t => _actionEntityBLLMapper.Map(t)).ToList()!
            
        };
        return res;
    }

    public DTO.Supplier? Map(Supplier? entity)
    {
        if (entity == null) return null;
        
        var res = new DTO.Supplier()
        {
            Id = entity.Id,
            Name = entity.Name,
            TelephoneNr = entity.TelephoneNr,
            Email = entity.Email,
            
            AddressId = entity.AddressId,
            Address = AddressBLLMapper.MapSimple(entity.Address),
            
            Actions = entity.Actions?.Select(t => _actionEntityBLLMapper.Map(t)).ToList()!
            
        };
        return res;
    }
    
    public static Supplier? MapSimple(DTO.Supplier? entity)
    {
        if (entity == null) return null;

        return new Supplier()
        {
            Id = entity.Id,
            Name = entity.Name,
            TelephoneNr = entity.TelephoneNr,
            Email = entity.Email,
            AddressId = entity.AddressId,
        };
    }
    
    public static DTO.Supplier? MapSimple(Supplier? entity)
    {
        if (entity == null) return null;

        return new DTO.Supplier()
        {
            Id = entity.Id,
            Name = entity.Name,
            TelephoneNr = entity.TelephoneNr,
            Email = entity.Email,
            AddressId = entity.AddressId,
        };
    }
}