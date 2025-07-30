using App.DAL.DTO;
using Base.Contracts;
using Base.DAL.Contracts;

namespace App.DAL.Contracts;

public interface IRecipeComponentRepository: IBaseRepository<RecipeComponent>
{
    Task<List<Domain.Logic.RecipeComponent>> GetComponentsByRecipeProductIdAsync(Guid id);
}