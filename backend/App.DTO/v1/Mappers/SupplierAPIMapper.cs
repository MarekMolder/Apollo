using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class SupplierAPIMapper : IMapper<Supplier, BLL.DTO.Supplier>
{
    public Supplier? Map(BLL.DTO.Supplier? entity)
    {
        if (entity == null) return null;
        var res = new Supplier
        {
            Id = entity.Id,
            Name = entity.Name,
            TelephoneNr = entity.TelephoneNr,
            Email = entity.Email,
            AddressId = entity.AddressId,
        };
        return res;
    }

    public BLL.DTO.Supplier? Map(Supplier? entity)
    {
        if (entity == null) return null;
        var res = new BLL.DTO.Supplier
        {
            Id = entity.Id,
            Name = entity.Name,
            TelephoneNr = entity.TelephoneNr,
            Email = entity.Email,
            AddressId = entity.AddressId,
        };
        return res;
    }
    
    public BLL.DTO.Supplier Map(SupplierCreate entity)
    {
        var res = new BLL.DTO.Supplier
        {
            Id = Guid.NewGuid(),
            Name = entity.Name,
            TelephoneNr = entity.TelephoneNr,
            Email = entity.Email,
            AddressId = entity.AddressId,
        };
        return res;
    }
}