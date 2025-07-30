using Base.DAL.Contracts;

namespace App.DAL.Contracts;

/// <summary>
/// Repository interface for handling data access related to StorageRooms.
/// </summary>
public interface IStorageRoomRepository: IBaseRepository<DAL.DTO.StorageRoom>
{
    /// <summary>
    /// Retrieves StorageRoom enriched with related data.
    /// </summary>
    Task<IEnumerable<DAL.DTO.StorageRoom?>> GetEnrichedStorageRooms();
}