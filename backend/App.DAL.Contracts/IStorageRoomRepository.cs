using App.DAL.DTO;
using Base.DAL.Contracts;

namespace App.DAL.Contracts;

public interface IStorageRoomRepository: IBaseRepository<StorageRoom>
{
    Task<IEnumerable<StorageRoom>> GetAllByInventoryIdAsync(Guid inventoryId);
}