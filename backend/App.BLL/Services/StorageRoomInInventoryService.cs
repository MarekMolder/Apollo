using App.BLL.Contracts;
using App.BLL.Mappers;
using App.DAL.Contracts;
using App.DAL.DTO;
using Base.BLL;
using Base.BLL.Contracts;
using Base.DAL.Contracts;

namespace App.BLL.Services;

public class StorageRoomInInventoryService : BaseService<App.BLL.DTO.StorageRoomInInventory, App.DAL.DTO.StorageRoomInInventory, App.DAL.Contracts.IStorageRoomInInventoryRepository>, IStorageRoomInInventoryService
{
    public StorageRoomInInventoryService(IAppUOW serviceUow, StorageRoomInInventoryBLLMapper bllMapper) : base(serviceUow, serviceUow.StorageRoomInInventoryRepository, bllMapper)
    {
    }
}