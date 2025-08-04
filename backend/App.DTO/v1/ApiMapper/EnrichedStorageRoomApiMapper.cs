using App.DTO.v1.ApiEntities;
using Base.Contracts;

namespace App.DTO.v1.ApiMapper;

/// <summary>
/// Maps between DTO V1 and BLL DTO StorageRoom objects.
/// Includes both full and simplified mapping logic.
/// </summary>
public class EnrichedStorageRoomApiMapper : IMapper<EnrichedStorageRoom, BLL.DTO.StorageRoom>
{
    /// <summary>
    /// Maps a full BLL DTO StorageRoom to a DTO V1 StorageRoom, including related objects.
    /// </summary>
    public EnrichedStorageRoom? Map(BLL.DTO.StorageRoom? entity)
    {
        if (entity == null) return null;

        var address = entity.Address;

        var fullAddress = address != null
            ? $"{(string.IsNullOrWhiteSpace(address.Name) ? "" : address.Name + ", ")}{address.StreetName} {address.BuildingNr}{(address.UnitNr != null ? "-" + address.UnitNr : "")}, {address.PostalCode} {address.City}, {address.Province}, {address.Country}"
            : "Unknown";

        return new EnrichedStorageRoom
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            AddressId = entity.AddressId,
            FullAddress = fullAddress,
            AllowedRoles = entity.AllowedRoles?.ToList()
        };
    }

    public BLL.DTO.StorageRoom? Map(EnrichedStorageRoom? entity)
    {
        throw new NotImplementedException("Mapping from API to BLL is not required.");
    }
}