using App.DAL.DTO;
using App.DAL.EF.Mappers;
using Base.Contracts;

namespace App.BLL.Mappers;

public class RecipeComponentBLLMapper: IMapper<App.BLL.DTO.RecipeComponent, RecipeComponent>
{
    public RecipeComponent? Map(DTO.RecipeComponent? entity)
    {
        if (entity == null) return null;
        
        var res = new RecipeComponent()
        {
            Id = entity.Id,
            
            ProductRecipeId = entity.ProductRecipeId,
            ProductRecipe = ProductBLLMapper.MapSimple(entity.ProductRecipe),
            
            ComponentProductId = entity.ComponentProductId,
            ComponentProduct = ProductBLLMapper.MapSimple(entity.ComponentProduct),
            
            Amount = entity.Amount,
        };
        return res;
    }

    public DTO.RecipeComponent? Map(RecipeComponent? entity)
    {
        if (entity == null) return null;
        
        var res = new DTO.RecipeComponent()
        {
            Id = entity.Id,
            
            ProductRecipeId = entity.ProductRecipeId,
            ProductRecipe = ProductBLLMapper.MapSimple(entity.ProductRecipe),
            
            ComponentProductId = entity.ComponentProductId,
            ComponentProduct = ProductBLLMapper.MapSimple(entity.ComponentProduct),
            
            Amount = entity.Amount,
        };
        return res;
    }
}