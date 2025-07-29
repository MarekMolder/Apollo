using Base.Contracts;

namespace App.DAL.DTO;

public class RecipeComponent : IDomainId
{
    public Guid Id { get; set; }

    public Guid ProductRecipeId { get; set; }
    public Product? ProductRecipe { get; set; }
    
    public Guid ComponentProductId { get; set; }
    public Product? ComponentProduct { get; set; }

    public decimal Amount { get; set; }
}