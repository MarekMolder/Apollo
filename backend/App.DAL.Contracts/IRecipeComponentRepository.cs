using Base.DAL.Contracts;

namespace App.DAL.Contracts;

/// <summary>
/// Repository interface for handling data access related to RecipeComponents.
/// </summary>
public interface IRecipeComponentRepository: IBaseRepository<DAL.DTO.RecipeComponent>
{
    /// <summary>
    /// Retrieves all components (ingredients) for the specified recipe product.
    /// </summary>
    Task<List<Domain.Logic.RecipeComponent>> GetComponentsByRecipeProductIdAsync(Guid id);
}