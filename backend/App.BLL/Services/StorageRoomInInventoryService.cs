using App.BLL.Contracts;
using App.BLL.DTO;
using App.BLL.Mappers;
using App.DAL.Contracts;
using Base.BLL;

namespace App.BLL.Services;

public class StorageRoomInInventoryService : BaseService<StorageRoomInInventory, DAL.DTO.StorageRoomInInventory, IStorageRoomInInventoryRepository>, IStorageRoomInInventoryService
{
    public StorageRoomInInventoryService(IAppUOW serviceUow, StorageRoomInInventoryBLLMapper bllMapper) : base(serviceUow, serviceUow.StorageRoomInInventoryRepository, bllMapper)
    {
    }
}