using Base.BLL.Contracts;

namespace App.BLL.Contracts;

/// <summary>
/// Business logic contract for managing StorageRoom operations.
/// </summary>
public interface IStorageRoomService : IBaseService<BLL.DTO.StorageRoom>
{
    /// <summary>
    /// Retrieves StorageRoom enriched with related data.
    /// </summary>
    Task<IEnumerable<BLL.DTO.StorageRoom?>> GetEnrichedStorageRooms();
}