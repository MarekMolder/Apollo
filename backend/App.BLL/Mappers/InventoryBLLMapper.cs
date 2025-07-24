using App.DAL.DTO;
using Base.Contracts;

namespace App.BLL.Mappers;

public class InventoryBLLMapper : IMapper<App.BLL.DTO.Inventory, Inventory>
{
    private readonly StorageRoomInInventoryBLLMapper _storageRoomInInventoryBllMapper = new();
    public Inventory? Map(DTO.Inventory? entity)
    {
        if (entity == null) return null;
        
        var res = new Inventory()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
          
            AddressId = entity.AddressId,
            Address = AddressBLLMapper.MapSimple(entity.Address),
            
            AllowedRoles = entity.AllowedRoles?.ToList(),
            
            StorageRoomInInventories = entity.StorageRoomInInventories?.Select(t => _storageRoomInInventoryBllMapper.Map(t)).ToList()!
            
        };
        return res;
    }

    public DTO.Inventory? Map(Inventory? entity)
    {
        if (entity == null) return null;
        
        var res = new DTO.Inventory()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
          
            AddressId = entity.AddressId,
            Address = AddressBLLMapper.MapSimple(entity.Address),
            
            AllowedRoles = entity.AllowedRoles?.ToList(),
            
            StorageRoomInInventories = entity.StorageRoomInInventories?.Select(t => _storageRoomInInventoryBllMapper.Map(t)).ToList()!
        };
        return res;
    }
    
    public static Inventory? MapSimple(DTO.Inventory? entity)
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
    
    public static DTO.Inventory? MapSimple(Inventory? entity)
    {
        if (entity == null) return null;

        return new DTO.Inventory()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            AddressId = entity.AddressId,
            AllowedRoles = entity.AllowedRoles?.ToList()
        };
    }
}