using App.BLL.Contracts;
using App.BLL.Mappers;
using App.DAL.Contracts;
using App.DAL.DTO;
using Base.BLL;
using Base.BLL.Contracts;
using Base.Contracts;
using Base.DAL.Contracts;

namespace App.BLL.Services;

public class CurrentStockService : BaseService<App.BLL.DTO.CurrentStock, App.DAL.DTO.CurrentStock, App.DAL.Contracts.ICurrentStockRepository>, ICurrentStockService
{
    private readonly IAppUOW _uow;
    private readonly IMapper<DTO.CurrentStock, CurrentStock> _dalToBLLMapper;
    public CurrentStockService(IAppUOW serviceUow, IMapper<DTO.CurrentStock, CurrentStock> mapper) : base(serviceUow, serviceUow.CurrentStockRepository, mapper)
    {
        _dalToBLLMapper = mapper;
        _uow = serviceUow;
    }
    
    public async Task<IEnumerable<App.BLL.DTO.CurrentStock?>> GetByStorageRoomIdAsync(Guid storageRoomId)
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