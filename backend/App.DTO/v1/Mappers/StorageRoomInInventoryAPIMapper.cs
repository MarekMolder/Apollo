using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class StorageRoomInInventoryAPIMapper : IMapper<App.DTO.v1.StorageRoomInInventory, App.BLL.DTO.StorageRoomInInventory>
{
    public App.DTO.v1.StorageRoomInInventory? Map(App.BLL.DTO.StorageRoomInInventory? entity)
    {
        if (entity == null) return null;
        var res = new App.DTO.v1.StorageRoomInInventory()
        {
            Id = entity.Id,
            EndedAt = entity.EndedAt,
            InventoryId = entity.InventoryId,
            StorageRoomId = entity.StorageRoomId,
        };
        return res;
    }

    public App.BLL.DTO.StorageRoomInInventory? Map(App.DTO.v1.StorageRoomInInventory? entity)
    {
        if (entity == null) return null;
        var res = new App.BLL.DTO.StorageRoomInInventory()
        {
            Id = entity.Id,
            EndedAt = entity.EndedAt,
            InventoryId = entity.InventoryId,
            StorageRoomId = entity.StorageRoomId,
        };
        return res;
    }
    
    public App.BLL.DTO.StorageRoomInInventory Map(App.DTO.v1.StorageRoomInInventoryCreate entity)
    {
        var res = new App.BLL.DTO.StorageRoomInInventory()
        {
            Id = Guid.NewGuid(),
            EndedAt = entity.EndedAt,
            InventoryId = entity.InventoryId,
            StorageRoomId = entity.StorageRoomId,
        };
        return res;
    }
}