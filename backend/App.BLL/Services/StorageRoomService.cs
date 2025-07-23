using App.BLL.Contracts;
using App.BLL.Mappers;
using App.DAL.Contracts;
using App.DAL.DTO;
using Base.BLL;
using Base.BLL.Contracts;
using Base.Contracts;
using Base.DAL.Contracts;

namespace App.BLL.Services;

public class StorageRoomService : BaseService<App.BLL.DTO.StorageRoom, App.DAL.DTO.StorageRoom, App.DAL.Contracts.IStorageRoomRepository>, IStorageRoomService
{
    private readonly IMapper<DTO.StorageRoom, StorageRoom> _dalToBLLMapper;
    public StorageRoomService(IAppUOW serviceUow, IMapper<DTO.StorageRoom, StorageRoom> mapper) : base(serviceUow, serviceUow.StorageRoomRepository, mapper)
    {
        _dalToBLLMapper = mapper;
    }
    public async Task<IEnumerable<App.BLL.DTO.StorageRoom>> GetAllByInventoryIdAsync(Guid inventoryId)
    {
        var domainEntities = await ServiceRepository.GetAllByInventoryIdAsync(inventoryId);
        return domainEntities.Select(e => _dalToBLLMapper.Map(e)!);
    }
}