using App.BLL.Contracts;
using App.BLL.DTO;
using App.DAL.Contracts;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class CurrentStockService : BaseService<CurrentStock, DAL.DTO.CurrentStock, ICurrentStockRepository>, ICurrentStockService
{
    private readonly IAppUOW _uow;
    private readonly IMapper<CurrentStock, DAL.DTO.CurrentStock> _dalToBLLMapper;
    public CurrentStockService(IAppUOW serviceUow, IMapper<CurrentStock, DAL.DTO.CurrentStock> mapper) : base(serviceUow, serviceUow.CurrentStockRepository, mapper)
    {
        _dalToBLLMapper = mapper;
        _uow = serviceUow;
    }
    
    public async Task<IEnumerable<CurrentStock?>> GetByStorageRoomIdAsync(Guid storageRoomId)
    {
        var res = await ServiceRepository.GetByStorageRoomIdAsync(storageRoomId);
        return res.Select(u => _dalToBLLMapper.Map(u));
    }
    
    public Task<List<(Guid ProductId, string ProductName, decimal Quantity)>> GetLowestStockProductsAsync(int count)
    {
        return _uow.CurrentStockRepository.GetLowestStockProductsAsync(count);
    }
    
    public Task<decimal> GetTotalInventoryWorthAsync(Guid? inventoryId = null)
    {
        return _uow.CurrentStockRepository.GetTotalInventoryWorthAsync(inventoryId);
    }

}