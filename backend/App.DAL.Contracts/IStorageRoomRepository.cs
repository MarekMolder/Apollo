using App.Resources.Domain;
using Base.DAL.Contracts;
using StorageRoom = App.Domain.Logic.StorageRoom;

namespace App.DAL.Contracts;

public interface IStorageRoomRepository: IBaseRepository<App.DAL.DTO.StorageRoom>
{
    Task<IEnumerable<App.DAL.DTO.StorageRoom>> GetAllByInventoryIdAsync(Guid inventoryId);
}