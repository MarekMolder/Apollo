using App.BLL.Contracts;
using App.DAL.Contracts;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

/// <summary>
/// Business logic service for managing RecipeComponents.
/// Provides enriched storage room data used in UI and reporting.
/// </summary>
public class StorageRoomService : BaseService<BLL.DTO.StorageRoom, DAL.DTO.StorageRoom, IStorageRoomRepository>, IStorageRoomService
{
    private readonly IMapper<BLL.DTO.StorageRoom, DAL.DTO.StorageRoom> _dalBllMapperStorageRooms;
    public StorageRoomService(IAppUOW serviceUow, IMapper<BLL.DTO.StorageRoom, DAL.DTO.StorageRoom> bllMapperStorageRooms) : base(serviceUow, serviceUow.StorageRoomRepository, bllMapperStorageRooms)
    {
        _dalBllMapperStorageRooms = bllMapperStorageRooms;
    }
    
    /// <summary>
    /// Returns StoragerRoom enriched with related data (joins from DB).
    /// </summary>
    public async Task<IEnumerable<BLL.DTO.StorageRoom?>> GetEnrichedStorageRooms()
    {
        var res = await ServiceRepository.GetEnrichedStorageRooms();
        return res.Select(u => _dalBllMapperStorageRooms.Map(u));
    }
}
