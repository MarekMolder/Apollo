using System.Runtime.InteropServices.Marshalling;
using System.Security.AccessControl;
using App.BLL.Contracts;
using App.BLL.Mappers;
using App.DAL.Contracts;
using App.DAL.DTO;
using App.BLL.DTO.Enums;
using Base.BLL;
using Base.BLL.Contracts;
using Base.Contracts;
using Base.DAL.Contracts;

namespace App.BLL.Services;

public class ActionEntityService : BaseService<App.BLL.DTO.ActionEntity, App.DAL.DTO.ActionEntity, App.DAL.Contracts.IActionEntityRepository>, IActionEntityService
{
    private readonly IAppUOW _uow;
    private readonly IMapper<App.DAL.DTO.CurrentStock, App.Domain.Logic.CurrentStock> _domainToDalMapperCurrentStock;
    private readonly IMapper<DTO.ActionEntity, ActionEntity> _dalToBLLMapper;
    private readonly IMapper<App.DAL.DTO.ActionEntity, App.Domain.Logic.ActionEntity> _domainToDalMapper;
    
    public ActionEntityService(
        IAppUOW serviceUow, 
        IMapper<DTO.ActionEntity, ActionEntity> mapper,
        IMapper<App.DAL.DTO.CurrentStock, App.Domain.Logic.CurrentStock> domainToDalMapperCurrentStock,
        IMapper<App.DAL.DTO.ActionEntity, App.Domain.Logic.ActionEntity> domainToDalMapper)
        : base(serviceUow, serviceUow.ActionEntityRepository, mapper)
    {
        _uow = serviceUow;
        _domainToDalMapperCurrentStock = domainToDalMapperCurrentStock;
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

            var currentStock = await _uow.CurrentStockRepository
                .FindByProductAndStorageAsync(productId, storageRoomId);
            
            var mappedCurrentStock = _domainToDalMapperCurrentStock.Map(currentStock);
            
            decimal quantityChange = bllAction.ActionType!.Code switch
            {
                ActionTypeEnum.Add => bllAction.Quantity,
                ActionTypeEnum.Remove => -bllAction.Quantity,
                _ => throw new InvalidOperationException("Unknown action type")
            };

            if (mappedCurrentStock != null)
            {
                mappedCurrentStock.Quantity += quantityChange;
                await _uow.CurrentStockRepository.UpdateAsync(mappedCurrentStock);
            }
            else
            {
                var newStock = new App.DAL.DTO.CurrentStock
                {
                    Id = Guid.NewGuid(),
                    ProductId = productId,
                    StorageRoomId = storageRoomId,
                    Quantity = quantityChange
                };
                await _uow.CurrentStockRepository.AddAsync(newStock);
            }
        }

        return true;
    }

    public async Task<IEnumerable<App.BLL.DTO.ActionEntity?>> GetEnrichedActionEntities()
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