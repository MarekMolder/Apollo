using App.BLL.Contracts;
using App.BLL.DTO;
using App.BLL.Mappers;
using App.DAL.Contracts;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

/// <summary>
/// Business logic service for managing RecipeComponents.
/// </summary>
public class RecipeComponentService : BaseService<BLL.DTO.RecipeComponent, DAL.DTO.RecipeComponent, IRecipeComponentRepository>, IRecipeComponentService
{
    // Maps between DAL.DTO and BLL.DTO RecipeComponents
    private readonly IMapper<BLL.DTO.RecipeComponent, DAL.DTO.RecipeComponent> _dalBllMapperRecipeComponents;
    
    public RecipeComponentService(IAppUOW serviceUow, IMapper<BLL.DTO.RecipeComponent, DAL.DTO.RecipeComponent> mapperRecipeComponents) : base(serviceUow, serviceUow.RecipeComponentRepository, mapperRecipeComponents)
    {
        _dalBllMapperRecipeComponents = mapperRecipeComponents;
    }
    
    /// <summary>
    /// Returns RecipeComponents enriched with related data (joins from DB).
    /// </summary>
    public async Task<IEnumerable<BLL.DTO.RecipeComponent?>> GetEnrichedRecipeComponents()
    {
        var res = await ServiceRepository.GetEnrichedRecipeComponents();
        return res.Select(u => _dalBllMapperRecipeComponents.Map(u));
    }
}