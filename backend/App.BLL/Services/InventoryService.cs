using App.BLL.Contracts;
using App.BLL.Mappers;
using App.DAL.Contracts;
using App.DAL.DTO;
using Base.BLL;
using Base.BLL.Contracts;
using Base.Contracts;
using Base.DAL.Contracts;

namespace App.BLL.Services;

public class InventoryService : BaseService<App.BLL.DTO.Inventory, App.DAL.DTO.Inventory, App.DAL.Contracts.IInventoryRepository>, IInventoryService
{
    private readonly IMapper<DTO.Inventory, Inventory> _dalToBLLMapper;
    public InventoryService(IAppUOW serviceUow, IMapper<DTO.Inventory, Inventory> mapper) : base(serviceUow, serviceUow.InventoryRepository, mapper)
    {
        _dalToBLLMapper = mapper;
    }
    public async Task<IEnumerable<App.BLL.DTO.Inventory?>> GetEnrichedInventories()
    {
        var res = await ServiceRepository.GetEnrichedInventories();
        return res.Select(u => _dalToBLLMapper.Map(u));
    }
}