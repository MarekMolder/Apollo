using Base.BLL.Contracts;

namespace App.BLL.Contracts;

public interface IStorageRoomService : IBaseService<App.BLL.DTO.StorageRoom>
{
    Task<IEnumerable<App.BLL.DTO.StorageRoom>> GetAllByInventoryIdAsync(Guid inventoryId);
}