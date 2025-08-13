using Base.BLL.Contracts;

namespace App.BLL.Contracts;

/// <summary>
/// Business logic contract for managing RecipeComponents operations.
/// </summary>
public interface IRecipeComponentService : IBaseService<BLL.DTO.RecipeComponent>
{
    /// <summary>
    /// Retrieves RecipeComponents enriched with related data.
    /// </summary>
    Task<IEnumerable<BLL.DTO.RecipeComponent?>> GetEnrichedRecipeComponents();
}