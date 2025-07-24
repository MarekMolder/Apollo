using App.DAL.DTO;
using Base.Contracts;

namespace App.DAL.EF.Mappers;

public class StorageRoomInInventoryUOWMapper: IMapper<StorageRoomInInventory, Domain.Logic.StorageRoomInInventory>
{
    public StorageRoomInInventory? Map(Domain.Logic.StorageRoomInInventory? entity)
    {
        if (entity == null) return null;
        
        var res = new StorageRoomInInventory
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
        
        var res = new Domain.Logic.StorageRoomInInventory
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
