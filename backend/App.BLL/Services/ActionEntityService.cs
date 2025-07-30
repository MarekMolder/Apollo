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

            // Leia retsepti komponendid, kui neid on
            var recipeComponents = await _uow.RecipeComponentRepository
                .GetComponentsByRecipeProductIdAsync(productId); // <-- Tee see uus meetod repo-sse

            if (recipeComponents != null && recipeComponents.Any())
            {
                // Kui on tegemist retseptitootega, tee iga komponendi jaoks uus maha kandmise action
                foreach (var component in recipeComponents)
                {
                    var componentQuantity =
                        bllAction.Quantity * component.Amount / 1m; // eeldame et Amount on "per unit"

                    // Värskenda ka MonthlyStatistics iga komponendi kohta
                    var compStat = await _uow.MonthlyStatisticsRepository
                        .FindByProductAndStorageAsync(component.ComponentProductId, bllAction.StorageRoomId);

                    if (compStat != null)
                    {
                        var mappedCompStat = _domainToDalMapperMonthlyStatistics.Map(compStat);
                        mappedCompStat!.TotalRemovedQuantity += componentQuantity;
                        await _uow.MonthlyStatisticsRepository.UpdateAsync(mappedCompStat);
                    }
                    else
                    {
                        var newStat = new DAL.DTO.MonthlyStatistics
                        {
                            Id = Guid.NewGuid(),
                            ProductId = component.ComponentProductId,
                            StorageRoomId = bllAction.StorageRoomId,
                            TotalRemovedQuantity = componentQuantity,
                            Year = DateTime.Today.Year,
                            Month = DateTime.Today.Month,
                        };
                        await _uow.MonthlyStatisticsRepository.AddAsync(newStat);
                    }
                }

                // Retsepti enda kohta ei tee maha kandmist – exit siit ära
                return true;
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