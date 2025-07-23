using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class AddressAPIMapper : IMapper<App.DTO.v1.Address, App.BLL.DTO.Address>
{
    public App.DTO.v1.Address? Map(App.BLL.DTO.Address? entity)
    {
        if (entity == null) return null;
        var res = new App.DTO.v1.Address()
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

    public App.BLL.DTO.Address? Map(App.DTO.v1.Address? entity)
    {
        if (entity == null) return null;
        var res = new App.BLL.DTO.Address()
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
    
    public App.BLL.DTO.Address Map(App.DTO.v1.AddressCreate entity)
    {
        var res = new App.BLL.DTO.Address()
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