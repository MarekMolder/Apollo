using App.BLL.Contracts;
using App.BLL.DTO;
using App.BLL.Mappers;
using App.DAL.Contracts;
using Base.BLL;

namespace App.BLL.Services;

/// <summary>
/// Business logic service for managing RecipeComponents.
/// </summary>
public class RecipeComponentService : BaseService<BLL.DTO.RecipeComponent, DAL.DTO.RecipeComponent, IRecipeComponentRepository>, IRecipeComponentService
{
    public RecipeComponentService(IAppUOW serviceUow, RecipeComponentBllMapper bllMapperRecipeComponents) : base(serviceUow, serviceUow.RecipeComponentRepository, bllMapperRecipeComponents)
    {
    }
}