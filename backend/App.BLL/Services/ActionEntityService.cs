using App.BLL.Contracts;
using App.BLL.DTO;
using App.BLL.DTO.Enums;
using App.DAL.Contracts;
using Base.BLL;
using Base.Contracts;
using CurrentStock = App.DAL.DTO.CurrentStock;
using MonthlyStatistics = App.DAL.DTO.MonthlyStatistics;

namespace App.BLL.Services;

public class ActionEntityService : BaseService<ActionEntity, DAL.DTO.ActionEntity, IActionEntityRepository>, IActionEntityService
{
    private readonly IAppUOW _uow;
    private readonly IMapper<DAL.DTO.MonthlyStatistics, Domain.Logic.MonthlyStatistics> _domainToDalMapperMonthlyStatistics;
    private readonly IMapper<ActionEntity, DAL.DTO.ActionEntity> _dalToBLLMapper;
    private readonly IMapper<DAL.DTO.ActionEntity, Domain.Logic.ActionEntity> _domainToDalMapper;
    
    public ActionEntityService(
        IAppUOW serviceUow, 
        IMapper<ActionEntity, DAL.DTO.ActionEntity> mapper,
        IMapper<MonthlyStatistics, Domain.Logic.MonthlyStatistics> domainToDalMapperMonthlyStatistics,
        IMapper<DAL.DTO.ActionEntity, Domain.Logic.ActionEntity> domainToDalMapper)
        : base(serviceUow, serviceUow.ActionEntityRepository, mapper)
    {
        _uow = serviceUow;
        _domainToDalMapperMonthlyStatistics = domainToDalMapperMonthlyStatistics;
        _dalToBLLMapper = mapper;
        _domainToDalMapper = domainToDalMapper;
    }
    
    public virtual async Task<bool> UpdateStatusAsync(Guid id, string newStatus)
    {
        var action = await _uow.ActionEntityRepository.FindAsync(id);
        if (action == null) return false;

        var allowedStatuses = new[] { "Accepted", "Declined" };
        if (!allowedStatuses.Contains(newStatus)) throw new ArgumentException("Invalid status");

        action.Status = newStatus;

        var dalAction = _domainToDalMapper.Map(action);
        var bllAction = _dalToBLLMapper.Map(dalAction);
            
        await UpdateAsync(bllAction);
        
        
        if (newStatus == "Accepted")
        {
            var productId = bllAction.ProductId;
            var storageRoomId = bllAction.StorageRoomId;

            var monthlyStatistics = await _uow.MonthlyStatisticsRepository
                .FindByProductAndStorageAsync(productId, storageRoomId);
            
            var mappedMonthlyStatistics = _domainToDalMapperMonthlyStatistics.Map(monthlyStatistics);
            
            decimal quantityChange = bllAction.ActionType!.Code switch
            {
                ActionTypeEnum.Add => bllAction.Quantity,
                ActionTypeEnum.Remove => -bllAction.Quantity,
                _ => throw new InvalidOperationException("Unknown action type")
            };

            if (mappedMonthlyStatistics != null)
            {
                mappedMonthlyStatistics.TotalRemovedQuantity += quantityChange;
                await _uow.MonthlyStatisticsRepository.UpdateAsync(mappedMonthlyStatistics);
            }
            else
            {
                var newMonthlyStatistics = new DAL.DTO.MonthlyStatistics
                {
                    Id = Guid.NewGuid(),
                    ProductId = productId,
                    StorageRoomId = storageRoomId,
                    TotalRemovedQuantity = quantityChange,
                    Year = DateTime.Today.Year,
                    Month = DateTime.Today.Month,
                };
                await _uow.MonthlyStatisticsRepository.AddAsync(newMonthlyStatistics);
            }
        }

        return true;
    }

    public async Task<IEnumerable<ActionEntity?>> GetEnrichedActionEntities()
    {
        var res = await ServiceRepository.GetEnrichedActionEntities();
        return res.Select(u => _dalToBLLMapper.Map(u));
    }
    
    public async Task<IEnumerable<(Guid ProductId, string ProductName, decimal RemoveQuantity)>> GetTopRemovedProductsAsync()
    {
        return await _uow.ActionEntityRepository.GetTopRemovedProductsAsync();
    }
    
    public async Task<IEnumerable<(string CreatedBy, decimal TotalRemovedQuantity)>> GetTopUsersByRemovedQuantityAsync()
    {
        return await _uow.ActionEntityRepository.GetTopUsersByRemovedQuantityAsync();
    }
    
} 