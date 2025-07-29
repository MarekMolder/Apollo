using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class RecipeComponentAPIMapper : IMapper<RecipeComponent, BLL.DTO.RecipeComponent>
{
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