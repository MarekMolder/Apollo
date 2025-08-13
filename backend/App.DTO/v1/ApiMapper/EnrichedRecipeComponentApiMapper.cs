using App.DTO.v1.ApiEntities;
using Base.Contracts;

namespace App.DTO.v1.ApiMapper;

/// <summary>
/// Maps between DTO V1 and BLL DTO RecipeComponent objects.
/// Includes both full and simplified mapping logic.
/// </summary>
public class EnrichedRecipeComponentApiMapper : IMapper<EnrichedRecipeComponent, BLL.DTO.RecipeComponent>
{
    /// <summary>
    /// Maps a full BLL DTO RecipeComponent to a DTO V1 RecipeComponent, including related objects.
    /// </summary>
    public EnrichedRecipeComponent? Map(BLL.DTO.RecipeComponent? entity)
    {
        if (entity == null) return null;

        return new EnrichedRecipeComponent
        {
            Id = entity.Id,
            
            ProductRecipeId = entity.ProductRecipeId,
            ProductRecipeCode = entity.ProductRecipe?.Code ?? "Unknown",
            ProductRecipeName = entity.ProductRecipe?.Name ?? "Unknown",
            ProductRecipeUnit = entity.ProductRecipe?.Unit ?? "Unknown",
            ProductRecipeVolume = entity.ProductRecipe?.Volume ?? 0m,
            ProductRecipeVolumeUnit = entity.ProductRecipe?.VolumeUnit ?? "Unknown",
            
            ComponentProductId = entity.ComponentProductId,
            ComponentProductCode = entity.ComponentProduct?.Code ?? "Unknown",
            ComponentProductName = entity.ComponentProduct?.Name ?? "Unknown",
            ComponentProductUnit = entity.ComponentProduct?.Unit ?? "Unknown",
            ComponentProductVolume = entity.ComponentProduct?.Volume ?? 0m,
            ComponentProductVolumeUnit = entity.ComponentProduct?.VolumeUnit ?? "Unknown",
            
            Amount = entity.Amount,
        };
    }

    public BLL.DTO.RecipeComponent? Map(EnrichedRecipeComponent? entity)
    {
        throw new NotImplementedException("Mapping from API to BLL is not required.");
    }
}