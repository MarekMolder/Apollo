using App.BLL.Contracts;
using App.BLL.DTO;
using App.BLL.Mappers;
using App.DAL.Contracts;
using Base.BLL;

namespace App.BLL.Services;

public class RecipeComponentService : BaseService<RecipeComponent, DAL.DTO.RecipeComponent, IRecipeComponentRepository>, IRecipeComponentService
{
    public RecipeComponentService(IAppUOW serviceUow, RecipeComponentBLLMapper bllMapper) : base(serviceUow, serviceUow.RecipeComponentRepository, bllMapper)
    {
    }
}