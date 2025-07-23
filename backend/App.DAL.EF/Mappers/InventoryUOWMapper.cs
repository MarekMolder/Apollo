using App.DAL.DTO;
using Base.Contracts;
using Base.DAL.Contracts;

namespace App.DAL.EF.Mappers;

public class InventoryUOWMapper: IMapper<App.DAL.DTO.Inventory, App.Domain.Logic.Inventory>
{
    private readonly StorageRoomInInventoryUOWMapper _storageRoomInInventoryUOWMapper = new();
    public Inventory? Map(Domain.Logic.Inventory? entity)
    {
        if (entity == null) return null;
        
        var res = new Inventory()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
          
            AddressId = entity.AddressId,
            Address = AddressUOWMapper.MapSimple(entity.Address),
            
            AllowedRoles = entity.AllowedRoles?.ToList(),
            
            StorageRoomInInventories = entity.StorageRoomInInventories?.Select(t => _storageRoomInInventoryUOWMapper.Map(t)).ToList()!
        };
        return res;
    }

    public Domain.Logic.Inventory? Map(Inventory? entity)
    {
        if (entity == null) return null;
        
        var res = new Domain.Logic.Inventory()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
          
            AddressId = entity.AddressId,
            Address = AddressUOWMapper.MapSimple(entity.Address),
            
            AllowedRoles = entity.AllowedRoles?.ToList(),
            
            StorageRoomInInventories = entity.StorageRoomInInventories?.Select(t => _storageRoomInInventoryUOWMapper.Map(t)).ToList()!
        };
        return res;
    }
    
    public static Inventory? MapSimple(Domain.Logic.Inventory? entity)
    {
        if (entity == null) return null;

        return new Inventory()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            AddressId = entity.AddressId,
            AllowedRoles = entity.AllowedRoles?.ToList()
        };
    }

    public static Domain.Logic.Inventory? MapSimple(Inventory? entity)
    {
        if (entity == null) return null;

        return new Domain.Logic.Inventory()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            AddressId = entity.AddressId,
            AllowedRoles = entity.AllowedRoles?.ToList()
        };
    }
}
