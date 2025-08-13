using Base.Contracts;

namespace App.DTO.v1.Mappers;

/// <summary>
/// Maps between DTO V1 and BLL DTO StorageRoom objects.
/// Includes both full and simplified mapping logic.
/// </summary>
public class StorageRoomApiMapper : IMapper<StorageRoom, BLL.DTO.StorageRoom>
{
    /// <summary>
    /// Maps a simplified BLL DTO StorageRoom to DTO V1 StorageRoom, without related objects.
    /// </summary>
    public StorageRoom? Map(BLL.DTO.StorageRoom? entity)
    {
        if (entity == null) return null;
        var res = new StorageRoom
        {
            Id = entity.Id,
            Name = entity.Name,
            AddressId = entity.AddressId,
            AllowedRoles = entity.AllowedRoles?.ToList(),
        };
        return res;
    }

    /// <summary>
    /// Maps a simplified DTO V1 StorageRoom to BLL DTO StorageRoom, without related objects.
    /// </summary>
    public BLL.DTO.StorageRoom? Map(StorageRoom? entity)
    {
        if (entity == null) return null;
        var res = new BLL.DTO.StorageRoom
        {
            Id = entity.Id,
            Name = entity.Name,
            AddressId = entity.AddressId,
            AllowedRoles = entity.AllowedRoles?.ToList(),
        };
        return res;
    }
    
    /// <summary>
    /// Maps a simplified DTO V1 StorageRoomCreate to BLL DTO StorageRoom, without related objects.
    /// </summary>
    public BLL.DTO.StorageRoom Map(StorageRoomCreate entity)
    {
        var res = new BLL.DTO.StorageRoom
        {
            Id = Guid.NewGuid(),
            Name = entity.Name,
            AddressId = entity.AddressId,
            AllowedRoles = entity.AllowedRoles?.ToList(),
        };
        return res;
    }
}