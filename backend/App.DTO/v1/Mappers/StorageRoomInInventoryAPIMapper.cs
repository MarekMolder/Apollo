using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class StorageRoomInInventoryAPIMapper : IMapper<StorageRoomInInventory, BLL.DTO.StorageRoomInInventory>
{
    public StorageRoomInInventory? Map(BLL.DTO.StorageRoomInInventory? entity)
    {
        if (entity == null) return null;
        var res = new StorageRoomInInventory
        {
            Id = entity.Id,
            EndedAt = entity.EndedAt,
            InventoryId = entity.InventoryId,
            StorageRoomId = entity.StorageRoomId,
        };
        return res;
    }

    public BLL.DTO.StorageRoomInInventory? Map(StorageRoomInInventory? entity)
    {
        if (entity == null) return null;
        var res = new BLL.DTO.StorageRoomInInventory
        {
            Id = entity.Id,
            EndedAt = entity.EndedAt,
            InventoryId = entity.InventoryId,
            StorageRoomId = entity.StorageRoomId,
        };
        return res;
    }
    
    public BLL.DTO.StorageRoomInInventory Map(StorageRoomInInventoryCreate entity)
    {
        var res = new BLL.DTO.StorageRoomInInventory
        {
            Id = Guid.NewGuid(),
            EndedAt = entity.EndedAt,
            InventoryId = entity.InventoryId,
            StorageRoomId = entity.StorageRoomId,
        };
        return res;
    }
}