using Base.Contracts;

namespace App.BLL.DTO;

/// <summary>
/// Represents a single component (ingredient) in a recipe product.
/// Links a main product to one of its required components and defines the quantity needed.
/// </summary>
public class RecipeComponent : IDomainId
{
    /// <summary>
    /// Unique identifier for the recipe component record.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// ID of the product that is made from this recipe (the parent product).
    /// Reference to the recipe product entity.
    /// </summary>
    public Guid ProductRecipeId { get; set; }
    public BLL.DTO.Product? ProductRecipe { get; set; }
    
    /// <summary>
    /// ID of the component product used in the recipe.
    /// Reference to the component (ingredient) product entity.
    /// </summary>
    public Guid ComponentProductId { get; set; }
    public BLL.DTO.Product? ComponentProduct { get; set; }

    /// <summary>
    /// Quantity of the component required to produce one unit of the recipe product.
    /// </summary>
    public decimal Amount { get; set; }
}