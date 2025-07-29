using Base.Domain;

namespace App.Domain.Logic;

public class RecipeComponent : BaseEntity
{
    public Guid ProductRecipeId { get; set; }
    public Product? ProductRecipe { get; set; }
    
    public Guid ComponentProductId { get; set; }
    public Product? ComponentProduct { get; set; }

    public decimal Amount { get; set; }
}