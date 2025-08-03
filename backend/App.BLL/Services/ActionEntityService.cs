using App.BLL.Contracts;
using App.BLL.Utils;
using App.DAL.Contracts;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

/// <summary>
/// Business logic service for managing ActionEntities.
/// Includes methods for updating statuses and handling recipe-based removals.
/// </summary>
public class ActionEntityService : BaseService<BLL.DTO.ActionEntity, DAL.DTO.ActionEntity, IActionEntityRepository>, IActionEntityService
{
    private readonly IAppUOW _uow;
    
    // Maps between DAL.DTO and Domain MonthlyStatistics
    private readonly IMapper<DAL.DTO.MonthlyStatistics, Domain.Logic.MonthlyStatistics> _domainDalMapperMonthlyStatistics;
    
    // Maps between BLL DTO and DAL DTO ActionEntity
    private readonly IMapper<BLL.DTO.ActionEntity, DAL.DTO.ActionEntity> _dalBllMapperActionEntity;
    
    // Maps between DAL.DTO and Domain ActionEntity
    private readonly IMapper<DAL.DTO.ActionEntity, Domain.Logic.ActionEntity> _domainDalMapperActionEntity;
    
    public ActionEntityService(
        IAppUOW serviceUow, 
        IMapper<BLL.DTO.ActionEntity, DAL.DTO.ActionEntity> mapperActionEntity,
        IMapper<DAL.DTO.MonthlyStatistics, Domain.Logic.MonthlyStatistics> domainToDalMapperMonthlyStatistics,
        IMapper<DAL.DTO.ActionEntity, Domain.Logic.ActionEntity> domainToDalMapperActionEntity)
        : base(serviceUow, serviceUow.ActionEntityRepository, mapperActionEntity)
    {
        _uow = serviceUow;
        _domainDalMapperMonthlyStatistics = domainToDalMapperMonthlyStatistics;
        _dalBllMapperActionEntity = mapperActionEntity;
        _domainDalMapperActionEntity = domainToDalMapperActionEntity;
    }

    /// <summary>
    /// Updates the status of a given ActionEntity (e.g., to "Accepted" or "Declined").
    /// 
    /// - If the new status is "Accepted" and the associated product is a recipe (i.e., it has recipe components),
    ///   the method automatically updates MonthlyStatistics for each component based on the original action's quantity.
    /// 
    /// - If MonthlyStatistics already exists for the component and storage room, it is incremented;
    ///   otherwise, a new entry is created.
    /// 
    /// Returns false if the entity is not found. Throws an exception if the status is invalid.
    /// </summary>
    public virtual async Task<bool> UpdateStatusAsync(Guid id, string newStatus)
    {
        var action = await _uow.ActionEntityRepository.FindAsync(id);
        if (action == null) return false;

        var allowedStatuses = new[] { "Accepted", "Declined" };
        if (!allowedStatuses.Contains(newStatus)) throw new ArgumentException("Invalid status");

        action.Status = newStatus;

        var dalAction = _domainDalMapperActionEntity.Map(action);
        var bllAction = _dalBllMapperActionEntity.Map(dalAction);
        await UpdateAsync(bllAction);
        
        if (newStatus == "Accepted")
        {
            var productId = bllAction.ProductId;
            
            var recipeComponents = await _uow.RecipeComponentRepository
                .GetComponentsByRecipeProductIdAsync(productId);

            if (recipeComponents != null && recipeComponents.Any())
            {
                foreach (var component in recipeComponents)
                {
                    // Leia põhitoote ühik
                    var baseProduct = await _uow.ProductRepository.FindAsync(bllAction.ProductId);
                    if (baseProduct == null) continue;

                    // Leia komponenti toote ühik
                    var componentProduct = await _uow.ProductRepository.FindAsync(component.ComponentProductId);
                    if (componentProduct == null) continue;

                    // Algne komponentkogus retseptis (nt 0.2 l õli)
                    var rawComponentQuantity = bllAction.Quantity * component.Amount;

                    // Teisenda, kui ühikud on erinevad
                    decimal convertedQuantity = rawComponentQuantity;
                    if (baseProduct.Unit != componentProduct.Unit)
                    {
                        try
                        {
                            convertedQuantity = UnitConverter.Convert(rawComponentQuantity, baseProduct.Unit, componentProduct.Unit);
                        }
                        catch (Exception e)
                        {
                            // Logi või ignoreeri — olenevalt vajadusest
                            continue;
                        }
                    }

                    var compStat = await _uow.MonthlyStatisticsRepository
                        .FindByProductAndStorageAsync(component.ComponentProductId, bllAction.StorageRoomId);

                    if (compStat != null)
                    {
                        var mappedCompStat = _domainDalMapperMonthlyStatistics.Map(compStat);
                        mappedCompStat!.TotalRemovedQuantity += convertedQuantity;
                        await _uow.MonthlyStatisticsRepository.UpdateAsync(mappedCompStat);
                    }
                    else
                    {
                        var newStat = new DAL.DTO.MonthlyStatistics
                        {
                            Id = Guid.NewGuid(),
                            ProductId = component.ComponentProductId,
                            StorageRoomId = bllAction.StorageRoomId,
                            TotalRemovedQuantity = convertedQuantity,
                            Year = DateTime.Today.Year,
                            Month = DateTime.Today.Month,
                        };
                        await _uow.MonthlyStatisticsRepository.AddAsync(newStat);
                    }
                }
            }
            else
            {
                var stat = await _uow.MonthlyStatisticsRepository
                    .FindByProductAndStorageAsync(productId, bllAction.StorageRoomId);

                if (stat != null)
                {
                    var mappedStat = _domainDalMapperMonthlyStatistics.Map(stat);
                    mappedStat!.TotalRemovedQuantity += bllAction.Quantity;
                    await _uow.MonthlyStatisticsRepository.UpdateAsync(mappedStat);
                }
                else
                {
                    var newStat = new DAL.DTO.MonthlyStatistics
                    {
                        Id = Guid.NewGuid(),
                        ProductId = productId,
                        StorageRoomId = bllAction.StorageRoomId,
                        TotalRemovedQuantity = bllAction.Quantity,
                        Year = DateTime.Today.Year,
                        Month = DateTime.Today.Month,
                    };
                    await _uow.MonthlyStatisticsRepository.AddAsync(newStat);
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Returns ActionEntities enriched with related data (joins from DB).
    /// </summary>
    public async Task<IEnumerable<BLL.DTO.ActionEntity?>> GetEnrichedActionEntities()
    {
        var res = await ServiceRepository.GetEnrichedActionEntities();
        return res.Select(u => _dalBllMapperActionEntity.Map(u));
    }
    
    /// <summary>
    /// Returns top products by removed quantity.
    /// </summary>
    public async Task<IEnumerable<(Guid ProductId, string ProductName, decimal RemoveQuantity)>> GetTopRemovedProductsAsync()
    {
        return await _uow.ActionEntityRepository.GetTopRemovedProductsAsync();
    }
    
    /// <summary>
    /// Returns users who have removed the most quantity across all actions.
    /// </summary>
    public async Task<IEnumerable<(string CreatedBy, decimal TotalRemovedQuantity)>> GetTopUsersByRemovedQuantityAsync()
    {
        return await _uow.ActionEntityRepository.GetTopUsersByRemovedQuantityAsync();
    }
}
