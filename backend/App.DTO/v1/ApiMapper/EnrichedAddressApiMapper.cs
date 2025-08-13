using App.DTO.v1.ApiEntities;
using Base.Contracts;

namespace App.DTO.v1.ApiMapper;

/// <summary>
/// Maps between DTO V1 and BLL DTO Address objects.
/// Includes both full and simplified mapping logic.
/// </summary>
public class EnrichedAddressApiMapper : IMapper<EnrichedAddress, BLL.DTO.Address>
{
    /// <summary>
    /// Maps a full BLL DTO Address to a DTO V1 Address, including related objects.
    /// </summary>
    public EnrichedAddress? Map(BLL.DTO.Address? entity)
    {
        if (entity == null) return null;

        var fullAddress =
            $"{(string.IsNullOrWhiteSpace(entity.Name) ? "" : entity.Name + ", ")}{entity.StreetName} {entity.BuildingNr}{(entity.UnitNr != null ? "-" + entity.UnitNr : "")}, {entity.PostalCode} {entity.City}, {entity.Province}, {entity.Country}";

        return new EnrichedAddress
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
            FullAddress = fullAddress,
        };
    }

    public BLL.DTO.Address? Map(EnrichedAddress? entity)
    {
        throw new NotImplementedException("Mapping from API to BLL is not required.");
    }
}