using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class AddressAPIMapper : IMapper<Address, BLL.DTO.Address>
{
    public Address? Map(BLL.DTO.Address? entity)
    {
        if (entity == null) return null;
        var res = new Address
        {
            Id = entity.Id,
            StreetName = entity.StreetName,
            BuildingNr = entity.BuildingNr,
            PostalCode = entity.PostalCode,
            City = entity.City,
            Province = entity.Province,
            Country = entity.Country,
            Name = entity.Name,
            UnitNr = entity.UnitNr,
        };
        return res;
    }

    public BLL.DTO.Address? Map(Address? entity)
    {
        if (entity == null) return null;
        var res = new BLL.DTO.Address
        {
            Id = entity.Id,
            StreetName = entity.StreetName,
            BuildingNr = entity.BuildingNr,
            PostalCode = entity.PostalCode,
            City = entity.City,
            Province = entity.Province,
            Country = entity.Country,
            Name = entity.Name,
            UnitNr = entity.UnitNr,
        };
        return res;
    }
    
    public BLL.DTO.Address Map(AddressCreate entity)
    {
        var res = new BLL.DTO.Address
        {
            Id = Guid.NewGuid(),
            StreetName = entity.StreetName,
            BuildingNr = entity.BuildingNr,
            PostalCode = entity.PostalCode,
            City = entity.City,
            Province = entity.Province,
            Country = entity.Country,
            Name = entity.Name,
            UnitNr = entity.UnitNr,
        };
        return res;
    }
}