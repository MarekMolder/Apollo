using App.DAL.DTO;
using Base.Contracts;

namespace App.DAL.EF.Mappers;

public class RecipeComponentUOWMapper: IMapper<RecipeComponent, Domain.Logic.RecipeComponent>
{    
    public RecipeComponent? Map(Domain.Logic.RecipeComponent? entity)
    {
        if (entity == null) return null;
        
        var res = new RecipeComponent
        {
            Id = entity.Id,
            
            ProductRecipeId = entity.ProductRecipeId,
            ProductRecipe = ProductUOWMapper.MapSimple(entity.ProductRecipe),
            
            ComponentProductId = entity.ComponentProductId,
            ComponentProduct = ProductUOWMapper.MapSimple(entity.ComponentProduct),
            
            Amount = entity.Amount,
        };
        return res;
    }

    public Domain.Logic.RecipeComponent? Map(RecipeComponent? entity)
    {
        if (entity == null) return null;
        
        var res = new Domain.Logic.RecipeComponent
        {
            Id = entity.Id,
            
            ProductRecipeId = entity.ProductRecipeId,
            ProductRecipe = ProductUOWMapper.MapSimple(entity.ProductRecipe),
            
            ComponentProductId = entity.ComponentProductId,
            ComponentProduct = ProductUOWMapper.MapSimple(entity.ComponentProduct),
            
            Amount = entity.Amount,
        };
        return res;
    }
}