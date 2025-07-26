using App.BLL.Contracts;
using App.BLL.DTO;
using App.DAL.Contracts;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class StorageRoomService : BaseService<StorageRoom, DAL.DTO.StorageRoom, IStorageRoomRepository>, IStorageRoomService
{
    private readonly IMapper<StorageRoom, DAL.DTO.StorageRoom> _dalToBLLMapper;
    public StorageRoomService(IAppUOW serviceUow, IMapper<StorageRoom, DAL.DTO.StorageRoom> mapper) : base(serviceUow, serviceUow.StorageRoomRepository, mapper)
    {
        _dalToBLLMapper = mapper;
    }
    public async Task<IEnumerable<StorageRoom?>> GetEnrichedStorageRooms()
    {
        var res = await ServiceRepository.GetEnrichedStorageRooms();
        return res.Select(u => _dalToBLLMapper.Map(u));
    }
}