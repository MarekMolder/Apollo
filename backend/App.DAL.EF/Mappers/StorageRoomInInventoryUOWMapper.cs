using App.DAL.DTO;
using Base.Contracts;
using Base.DAL.Contracts;

namespace App.DAL.EF.Mappers;

public class StorageRoomInInventoryUOWMapper: IMapper<App.DAL.DTO.StorageRoomInInventory, App.Domain.Logic.StorageRoomInInventory>
{
    public StorageRoomInInventory? Map(Domain.Logic.StorageRoomInInventory? entity)
    {
        if (entity == null) return null;
        
        var res = new StorageRoomInInventory()
        {
            Id = entity.Id,
            EndedAt = entity.EndedAt, 
            
            InventoryId = entity.InventoryId,
            Inventory = InventoryUOWMapper.MapSimple(entity.Inventory),
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomUOWMapper.MapSimple(entity.StorageRoom),
            
        };
        return res;
    }

    public Domain.Logic.StorageRoomInInventory? Map(StorageRoomInInventory? entity)
    {
        if (entity == null) return null;
        
        var res = new Domain.Logic.StorageRoomInInventory()
        {
            Id = entity.Id,
            EndedAt = entity.EndedAt, 
            
            InventoryId = entity.InventoryId,
            Inventory = InventoryUOWMapper.MapSimple(entity.Inventory),
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomUOWMapper.MapSimple(entity.StorageRoom),
            
        };
        return res;
    }
}
