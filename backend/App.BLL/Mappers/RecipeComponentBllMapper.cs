using Base.Contracts;

namespace App.BLL.Mappers;

/// <summary>
/// Maps between BLL DTO and DAL DTO RecipeComponent entities.
/// Handles both full mapping and simplified mapping.
/// </summary>
public class RecipeComponentBllMapper: IMapper<BLL.DTO.RecipeComponent, DAL.DTO.RecipeComponent>
{
    /// <summary>
    /// Maps a full BLL DTO RecipeComponent entity to a DAL DTO RecipeComponent entity, including related Products and ComponentProducts.
    /// </summary>
    public DAL.DTO.RecipeComponent? Map(BLL.DTO.RecipeComponent? entity)
    {
        if (entity == null) return null;
        
        var res = new DAL.DTO.RecipeComponent()
        {
            Id = entity.Id,
            
            ProductRecipeId = entity.ProductRecipeId,
            ProductRecipe = ProductBllMapper.MapSimple(entity.ProductRecipe),
            
            ComponentProductId = entity.ComponentProductId,
            ComponentProduct = ProductBllMapper.MapSimple(entity.ComponentProduct),
            
            Amount = entity.Amount,
        };
        return res;
    }

    /// <summary>
    /// Maps a full DAL DTO RecipeComponent entity to a BLL DTO RecipeComponent entity, including related Products and ComponentProducts.
    /// </summary>
    public BLL.DTO.RecipeComponent? Map(DAL.DTO.RecipeComponent? entity)
    {
        if (entity == null) return null;
        
        var res = new BLL.DTO.RecipeComponent()
        {
            Id = entity.Id,
            
            ProductRecipeId = entity.ProductRecipeId,
            ProductRecipe = ProductBllMapper.MapSimple(entity.ProductRecipe),
            
            ComponentProductId = entity.ComponentProductId,
            ComponentProduct = ProductBllMapper.MapSimple(entity.ComponentProduct),
            
            Amount = entity.Amount,
        };
        return res;
    }
}