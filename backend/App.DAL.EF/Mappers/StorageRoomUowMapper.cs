using Base.Contracts;

namespace App.DAL.EF.Mappers;

/// <summary>
/// Maps between Domain and DAL DTO StorageRoom entities.
/// Handles both full mapping and simplified mapping.
/// </summary>
public class StorageRoomUowMapper: IMapper<DAL.DTO.StorageRoom, Domain.Logic.StorageRoom>
{
    // Mapper used to map related ActionEntity objects
    private readonly ActionEntityUowMapper _actionEntityUowMapper = new();
    
    /// <summary>
    /// Maps a full Domain StorageRoom entity to a DAL DTO StorageRoom entity, including related Actions and Addresses.
    /// </summary>
    public DAL.DTO.StorageRoom? Map(Domain.Logic.StorageRoom? entity)
    {
        if (entity == null) return null;
        
        var res = new DAL.DTO.StorageRoom
        {
            Id = entity.Id,
            Name = entity.Name,
            AddressId = entity.AddressId,
            Address = AddressUowMapper.MapSimple(entity.Address),
            
            AllowedRoles = entity.AllowedRoles?.ToList(),
            
            Actions = entity.Actions?.Select(t => _actionEntityUowMapper.Map(t)).ToList()!,
        };
        return res;
    }

    /// <summary>
    /// Maps a full DAL DTO StorageRoom entity to a Domain StorageRoom entity, including related Actions and Addresses.
    /// </summary>
    public Domain.Logic.StorageRoom? Map(DAL.DTO.StorageRoom? entity)
    {
        if (entity == null) return null;
        
        var res = new Domain.Logic.StorageRoom
        {
            Id = entity.Id,
            Name = entity.Name,
            AddressId = entity.AddressId,
            Address = AddressUowMapper.MapSimple(entity.Address),
            
            AllowedRoles = entity.AllowedRoles?.ToList(),
            
            Actions = entity.Actions?.Select(t => _actionEntityUowMapper.Map(t)).ToList()!,
        };
        return res;
    }
    
    /// <summary>
    /// Maps a simplified Domain StorageRoom entity to a DAL DTO StorageRoom entity (no related Actions and Addresses).
    /// </summary>
    public static DAL.DTO.StorageRoom? MapSimple(Domain.Logic.StorageRoom? entity)
    {
        if (entity == null) return null;

        return new DAL.DTO.StorageRoom
        {
            Id = entity.Id,
            Name = entity.Name,
            AddressId = entity.AddressId,
            AllowedRoles = entity.AllowedRoles?.ToList()
        };
    }

    /// <summary>
    /// Maps a simplified DAL DTO StorageRoom entity to a Domain StorageRoom entity (no related Actions and Addresses).
    /// </summary>
    public static Domain.Logic.StorageRoom? MapSimple(DAL.DTO.StorageRoom? entity)
    {
        if (entity == null) return null;

        return new Domain.Logic.StorageRoom
        {
            Id = entity.Id,
            Name = entity.Name,
            AddressId = entity.AddressId,
            AllowedRoles = entity.AllowedRoles?.ToList()
        };
    }
}
