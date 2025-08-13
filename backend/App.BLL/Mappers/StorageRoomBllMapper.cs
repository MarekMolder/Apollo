using Base.Contracts;

namespace App.BLL.Mappers;

/// <summary>
/// Maps between BLL DTO and DAL DTO StorageRoom entities.
/// Handles both full mapping and simplified mapping.
/// </summary>
public class StorageRoomBllMapper : IMapper<BLL.DTO.StorageRoom, DAL.DTO.StorageRoom>
{
    // Mapper used to map related ActionEntity objects
    private readonly ActionEntityBllMapper _actionEntityBllMapper = new();
    
    /// <summary>
    /// Maps a full BLL DTO StorageRoom entity to a DAL DTO StorageRoom entity, including related Actions and Addresses.
    /// </summary>
    public DAL.DTO.StorageRoom? Map(BLL.DTO.StorageRoom? entity)
    {
        if (entity == null) return null;
        
        var res = new DAL.DTO.StorageRoom()
        {
            Id = entity.Id,
            Name = entity.Name,
            AddressId = entity.AddressId,
            Address = AddressBllMapper.MapSimple(entity.Address),
            
            AllowedRoles = entity.AllowedRoles?.ToList(),
            
            Actions = entity.Actions?.Select(t => _actionEntityBllMapper.Map(t)).ToList()!,
        };
        return res;
    }

    /// <summary>
    /// Maps a full DAL DTO StorageRoom entity to a BLL DTO StorageRoom entity, including related Actions and Addresses.
    /// </summary>
    public BLL.DTO.StorageRoom? Map(DAL.DTO.StorageRoom? entity)
    {
         if (entity == null) return null;
        
        var res = new BLL.DTO.StorageRoom()
        {
            Id = entity.Id,
            Name = entity.Name,
            AddressId = entity.AddressId,
            Address = AddressBllMapper.MapSimple(entity.Address),
            
            AllowedRoles = entity.AllowedRoles?.ToList(),
            
            Actions = entity.Actions?.Select(t => _actionEntityBllMapper.Map(t)).ToList()!,
        };
        return res;
    }
    
    /// <summary>
    /// Maps a simplified BLL DTO StorageRoom entity to a DAL DTO StorageRoom entity (no related Actions and Addresses).
    /// </summary>
    public static DAL.DTO.StorageRoom? MapSimple(BLL.DTO.StorageRoom? entity)
    {
        if (entity == null) return null;

        return new DAL.DTO.StorageRoom()
        {
            Id = entity.Id,
            Name = entity.Name,
            AddressId = entity.AddressId,
            AllowedRoles = entity.AllowedRoles?.ToList()
        };
    }
    
    /// <summary>
    /// Maps a simplified DAL DTO StorageRoom entity to a BLL DTO StorageRoom entity (no related Actions and Addresses).
    /// </summary>
    public static BLL.DTO.StorageRoom? MapSimple(DAL.DTO.StorageRoom? entity)
    {
        if (entity == null) return null;

        return new BLL.DTO.StorageRoom()
        {
            Id = entity.Id,
            Name = entity.Name,
            AddressId = entity.AddressId,
            AllowedRoles = entity.AllowedRoles?.ToList()
        };
    }
}
