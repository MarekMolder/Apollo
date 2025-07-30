using Base.Contracts;

namespace App.DTO.v1.Mappers;

/// <summary>
/// Maps between DTO V1 and BLL DTO Address objects.
/// Includes both full and simplified mapping logic.
/// </summary>
public class AddressApiMapper : IMapper<Address, BLL.DTO.Address>
{
    /// <summary>
    /// Maps a simplified BLL DTO Address to DTO V1 Address, without related objects.
    /// </summary>
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

    /// <summary>
    /// Maps a simplified DTO V1 Address to BLL DTO Address, without related objects.
    /// </summary>
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
    
    /// <summary>
    /// Maps a simplified DTO V1 AddressCreate to BLL DTO Address, without related objects.
    /// </summary>
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