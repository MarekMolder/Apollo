namespace App.DTO.v1;

public class RecipeComponentCreate
{
    public Guid ProductRecipeId { get; set; }
    public Guid ComponentProductId { get; set; }

    public decimal Amount { get; set; }
}