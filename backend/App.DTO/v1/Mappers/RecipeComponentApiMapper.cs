using Base.Contracts;

namespace App.DTO.v1.Mappers;

/// <summary>
/// Maps between DTO V1 and BLL DTO RecipeComponent objects.
/// Includes both full and simplified mapping logic.
/// </summary>
public class RecipeComponentApiMapper : IMapper<RecipeComponent, BLL.DTO.RecipeComponent>
{
    /// <summary>
    /// Maps a simplified BLL DTO RecipeComponent to DTO V1 RecipeComponent, without related objects.
    /// </summary>
    public RecipeComponent? Map(BLL.DTO.RecipeComponent? entity)
    {
        if (entity == null) return null;
        var res = new RecipeComponent
        {
            Id = entity.Id,
            ProductRecipeId = entity.ProductRecipeId,
            ComponentProductId = entity.ComponentProductId,
            Amount = entity.Amount,
        };
        return res;
    }

    /// <summary>
    /// Maps a simplified DTO V1 RecipeComponent to BLL DTO RecipeComponent, without related objects.
    /// </summary>
    public BLL.DTO.RecipeComponent? Map(RecipeComponent? entity)
    {
        if (entity == null) return null;
        var res = new BLL.DTO.RecipeComponent
        {
            Id = entity.Id,
            ProductRecipeId = entity.ProductRecipeId,
            ComponentProductId = entity.ComponentProductId,
            Amount = entity.Amount,
        };
        return res;
    }
    
    /// <summary>
    /// Maps a simplified DTO V1 RecipeComponentCreate to BLL DTO RecipeComponent, without related objects.
    /// </summary>
    public BLL.DTO.RecipeComponent Map(RecipeComponentCreate entity)
    {
        var res = new BLL.DTO.RecipeComponent
        {
            Id = Guid.NewGuid(),
            ProductRecipeId = entity.ProductRecipeId,
            ComponentProductId = entity.ComponentProductId,
            Amount = entity.Amount,
        };
        return res;
    }
}