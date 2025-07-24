using App.DTO.v1.ApiEntities;
using Base.Contracts;

namespace App.DTO.v1.ApiMappers;

public class EnrichedInventoryApiMapper : IMapper<EnrichedInventory, BLL.DTO.Inventory>
{
    public EnrichedInventory? Map(BLL.DTO.Inventory? entity)
    {
        if (entity == null) return null;

        var address = entity.Address;

        var fullAddress = address != null
            ? $"{(string.IsNullOrWhiteSpace(address.Name) ? "" : address.Name + ", ")}{address.StreetName} {address.BuildingNr}{(address.UnitNr != null ? "-" + address.UnitNr : "")}, {address.PostalCode} {address.City}, {address.Province}, {address.Country}"
            : "Unknown";

        return new EnrichedInventory
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            AddressId = entity.AddressId,
            FullAddress = fullAddress,
            AllowedRoles = entity.AllowedRoles?.ToList()
        };
    }

    public BLL.DTO.Inventory? Map(EnrichedInventory? entity)
    {
        throw new NotImplementedException("Mapping from API to BLL is not required.");
    }
    
}