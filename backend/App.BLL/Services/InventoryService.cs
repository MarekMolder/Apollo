using App.BLL.Contracts;
using App.BLL.DTO;
using App.DAL.Contracts;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class InventoryService : BaseService<Inventory, DAL.DTO.Inventory, IInventoryRepository>, IInventoryService
{
    private readonly IMapper<Inventory, DAL.DTO.Inventory> _dalToBLLMapper;
    public InventoryService(IAppUOW serviceUow, IMapper<Inventory, DAL.DTO.Inventory> mapper) : base(serviceUow, serviceUow.InventoryRepository, mapper)
    {
        _dalToBLLMapper = mapper;
    }
    public async Task<IEnumerable<Inventory?>> GetEnrichedInventories()
    {
        var res = await ServiceRepository.GetEnrichedInventories();
        return res.Select(u => _dalToBLLMapper.Map(u));
    }
}