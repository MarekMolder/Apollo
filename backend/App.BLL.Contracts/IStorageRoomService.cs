using Base.BLL.Contracts;

namespace App.BLL.Contracts;

public interface IStorageRoomService : IBaseService<DTO.StorageRoom>
{
    Task<IEnumerable<DTO.StorageRoom?>> GetEnrichedStorageRooms();
}