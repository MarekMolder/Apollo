using Base.Contracts;

namespace App.DTO.v1;

public class RecipeComponent : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid ProductRecipeId { get; set; }
    public Guid ComponentProductId { get; set; }

    public decimal Amount { get; set; }
}