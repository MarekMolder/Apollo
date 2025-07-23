using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class SupplierAPIMapper : IMapper<App.DTO.v1.Supplier, App.BLL.DTO.Supplier>
{
    public App.DTO.v1.Supplier? Map(App.BLL.DTO.Supplier? entity)
    {
        if (entity == null) return null;
        var res = new App.DTO.v1.Supplier()
        {
            Id = entity.Id,
            Name = entity.Name,
            TelephoneNr = entity.TelephoneNr,
            Email = entity.Email,
            AddressId = entity.AddressId,
        };
        return res;
    }

    public App.BLL.DTO.Supplier? Map(App.DTO.v1.Supplier? entity)
    {
        if (entity == null) return null;
        var res = new App.BLL.DTO.Supplier()
        {
            Id = entity.Id,
            Name = entity.Name,
            TelephoneNr = entity.TelephoneNr,
            Email = entity.Email,
            AddressId = entity.AddressId,
        };
        return res;
    }
    
    public App.BLL.DTO.Supplier Map(App.DTO.v1.SupplierCreate entity)
    {
        var res = new App.BLL.DTO.Supplier()
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