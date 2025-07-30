using Base.Contracts;

namespace App.DAL.EF.Mappers;

/// <summary>
/// Maps between Domain and DAL DTO RecipeComponent entities.
/// Handles both full mapping and simplified mapping.
/// </summary>
public class RecipeComponentUowMapper: IMapper<DAL.DTO.RecipeComponent, Domain.Logic.RecipeComponent>
{    
    /// <summary>
    /// Maps a full Domain RecipeComponent entity to a DAL DTO RecipeComponent entity, including related Products and ComponentProducts.
    /// </summary>
    public DAL.DTO.RecipeComponent? Map(Domain.Logic.RecipeComponent? entity)
    {
        if (entity == null) return null;
        
        var res = new DAL.DTO.RecipeComponent
        {
            Id = entity.Id,
            
            ProductRecipeId = entity.ProductRecipeId,
            ProductRecipe = ProductUowMapper.MapSimple(entity.ProductRecipe),
            
            ComponentProductId = entity.ComponentProductId,
            ComponentProduct = ProductUowMapper.MapSimple(entity.ComponentProduct),
            
            Amount = entity.Amount,
        };
        return res;
    }

    /// <summary>
    /// Maps a full DAL DTO RecipeComponent entity to a Domain RecipeComponent entity, including related Products and ComponentProducts.
    /// </summary>
    public Domain.Logic.RecipeComponent? Map(DAL.DTO.RecipeComponent? entity)
    {
        if (entity == null) return null;
        
        var res = new Domain.Logic.RecipeComponent
        {
            Id = entity.Id,
            
            ProductRecipeId = entity.ProductRecipeId,
            ProductRecipe = ProductUowMapper.MapSimple(entity.ProductRecipe),
            
            ComponentProductId = entity.ComponentProductId,
            ComponentProduct = ProductUowMapper.MapSimple(entity.ComponentProduct),
            
            Amount = entity.Amount,
        };
        return res;
    }
}