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
    public virtual async Task<bool> UpdateStatusAsync(Guid id, string newStatus, string currentUser, IEnumerable<string> roles)
{
    var action = await _uow.ActionEntityRepository.FindAsync(id);
    if (action == null) return false;

    if (string.Equals(action.Status, "Accepted", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(action.Status, "Declined", StringComparison.OrdinalIgnoreCase))
    {
        throw new InvalidOperationException("Status is already finalized and cannot be changed.");
    }
    
    var isWorker = roles.Any(r => r.Equals("töötaja", StringComparison.OrdinalIgnoreCase));
    var isManagerOrAdmin = roles.Any(r => r.Equals("juhataja", StringComparison.OrdinalIgnoreCase) || r.Equals("admin", StringComparison.OrdinalIgnoreCase));

    if (isWorker)
    {
        if (action.CreatedBy != currentUser)
            throw new UnauthorizedAccessException("You can only decline your own requests.");

        if (newStatus != "Declined")
            throw new ArgumentException("Workers can only decline their own requests.");
    }

    if (isManagerOrAdmin)
    {
        if (newStatus != "Accepted" && newStatus != "Declined")
            throw new ArgumentException("Invalid status for managers/admins.");
    }

    action.Status = newStatus;

    var dalAction = _domainDalMapperActionEntity.Map(action);
    var bllAction = _dalBllMapperActionEntity.Map(dalAction);
    await UpdateAsync(bllAction);

    if (newStatus != "Accepted") return true;

    var productId = bllAction.ProductId;
    
    var recipeComponents = await _uow.RecipeComponentRepository
        .GetComponentsByRecipeProductIdAsync(productId);

    if (recipeComponents != null && recipeComponents.Any())
    {
        var baseProduct = await _uow.ProductRepository.FindAsync(bllAction.ProductId);
        if (baseProduct == null) return true;

        foreach (var component in recipeComponents)
        {
            var componentProduct = await _uow.ProductRepository.FindAsync(component.ComponentProductId);
            if (componentProduct == null) continue;
            
            var rawComponentQuantity = bllAction.Quantity * component.Amount;

            var compStat = await _uow.MonthlyStatisticsRepository
                .FindByProductAndStorageAsync(component.ComponentProductId, bllAction.StorageRoomId);

            if (compStat != null)
            {
                var mappedCompStat = _domainDalMapperMonthlyStatistics.Map(compStat)!;
                mappedCompStat.TotalRemovedQuantity += rawComponentQuantity;
                
                if (mappedCompStat.ProductCategoryId == Guid.Empty)
                    mappedCompStat.ProductCategoryId = componentProduct.ProductCategoryId;

                await _uow.MonthlyStatisticsRepository.UpdateAsync(mappedCompStat);
            }
            else
            {
                var newStat = new DAL.DTO.MonthlyStatistics
                {
                    Id = Guid.NewGuid(),
                    ProductId = component.ComponentProductId,
                    ProductCategoryId = componentProduct.ProductCategoryId,
                    StorageRoomId = bllAction.StorageRoomId,
                    TotalRemovedQuantity = rawComponentQuantity,
                    Year = DateTime.Today.Year,
                    Month = DateTime.Today.Month,
                    Day = DateTime.Today.Day,
                };
                await _uow.MonthlyStatisticsRepository.AddAsync(newStat);
            }
        }
    }
    else
    {
        var product = await _uow.ProductRepository.FindAsync(productId);
        if (product == null) return true;

        var stat = await _uow.MonthlyStatisticsRepository
            .FindByProductAndStorageAsync(productId, bllAction.StorageRoomId);

        if (stat != null)
        {
            var mappedStat = _domainDalMapperMonthlyStatistics.Map(stat)!;
            mappedStat.TotalRemovedQuantity += bllAction.Quantity;
            
            if (mappedStat.ProductCategoryId == Guid.Empty)
                mappedStat.ProductCategoryId = product.ProductCategoryId;

            await _uow.MonthlyStatisticsRepository.UpdateAsync(mappedStat);
        }
        else
        {
            var newStat = new DAL.DTO.MonthlyStatistics
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                ProductCategoryId = product.ProductCategoryId,
                StorageRoomId = bllAction.StorageRoomId,
                TotalRemovedQuantity = bllAction.Quantity,
                Year = DateTime.Today.Year,
                Month = DateTime.Today.Month,
                Day = DateTime.Today.Day,
            };
            await _uow.MonthlyStatisticsRepository.AddAsync(newStat);
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
    /// Returns ActionEntities enriched filtered system.
    /// </summary>
    public async Task<IEnumerable<BLL.DTO.ActionEntity?>> GetEnrichedActionEntitiesFiltered(
        string? userEmail, int? month, int? year, string? status)
    {
        var res = await ServiceRepository.GetEnrichedActionEntities();

        var q = res.AsQueryable();

        if (!string.IsNullOrWhiteSpace(userEmail))
            q = q.Where(x => x!.CreatedBy == userEmail);

        if (year.HasValue)
            q = q.Where(x => x!.CreatedAt.Year == year.Value);

        if (month.HasValue)
            q = q.Where(x => x!.CreatedAt.Month == month.Value);

        if (!string.IsNullOrWhiteSpace(status) && (status == "Accepted" || status == "Declined" || status == "Pending"))
            q = q.Where(x => x!.Status == status);

        return q.Select(u => _dalBllMapperActionEntity.Map(u));
    }
    
    /// <summary>
    /// Returns top products by removed quantity.
    /// </summary>
    public async Task<List<(Guid ProductId, string ProductName, decimal RemoveQuantity, string ProductUnit, decimal? ProductVolume, string? ProductVolumeUnit)>> 
        GetTopRemovedProductsAsync(IEnumerable<string>? restrictToStorageRoles = null)
    {
        return await _uow.ActionEntityRepository.GetTopRemovedProductsAsync(restrictToStorageRoles);
    }
    
    /// <summary>
    /// Returns users who have removed the most quantity across all actions.
    /// </summary>
    public async Task<List<(string CreatedBy, int TotalRemovals)>> 
        GetTopUsersByRemovedQuantityAsync(IEnumerable<string>? restrictToStorageRoles = null)
    {
        return await _uow.ActionEntityRepository.GetTopUsersByRemovedQuantityAsync(restrictToStorageRoles);
    }
}
