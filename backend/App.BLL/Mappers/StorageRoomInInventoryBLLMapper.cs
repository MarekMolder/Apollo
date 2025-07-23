using App.DAL.DTO;
using Base.BLL.Contracts;
using Base.Contracts;

namespace App.BLL.Mappers;

public class StorageRoomInInventoryBLLMapper : IMapper<App.BLL.DTO.StorageRoomInInventory, App.DAL.DTO.StorageRoomInInventory>
{
    public StorageRoomInInventory? Map(DTO.StorageRoomInInventory? entity)
    {
        if (entity == null) return null;
        
        var res = new StorageRoomInInventory()
        {
            Id = entity.Id,
            EndedAt = entity.EndedAt, 
            
            InventoryId = entity.InventoryId,
            Inventory = InventoryBLLMapper.MapSimple(entity.Inventory),
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomBLLMapper.MapSimple(entity.StorageRoom),
            
        };
        return res;
    }

    public DTO.StorageRoomInInventory? Map(StorageRoomInInventory? entity)
    {
        if (entity == null) return null;
        
        var res = new DTO.StorageRoomInInventory()
        {
            Id = entity.Id,
            EndedAt = entity.EndedAt, 
            
            InventoryId = entity.InventoryId,
            Inventory = InventoryBLLMapper.MapSimple(entity.Inventory),
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomBLLMapper.MapSimple(entity.StorageRoom),
            
        };
        return res;
    }
}