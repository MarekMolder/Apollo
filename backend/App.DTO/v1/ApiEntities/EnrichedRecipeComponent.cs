using Base.Contracts;

namespace App.DTO.v1.ApiEntities;

/// <summary>
/// Represents a single component (ingredient) in a recipe product.
/// Links a main product to one of its required components and defines the quantity needed.
/// </summary>
public class EnrichedRecipeComponent : IDomainId
{
    /// <summary>
    /// Unique identifier for the recipe component record.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// ID of the product that is made from this recipe (the parent product).
    /// Name of the recipe product.
    /// Product code (SKU or internal identifier).
    /// Unit of measurement used for the  recipe product (e.g., "g", "ml").
    /// Total volume of the  recipe product.
    /// Measurement unit for the Volume field (e.g., "l", "ml", "g", "kg").
    /// </summary>
    public Guid ProductRecipeId { get; set; }
    public string ProductRecipeName { get; set; } = default!;
    public string ProductRecipeCode { get; set; } = default!;
    public string ProductRecipeUnit { get; set; } = default!;
    public decimal ProductRecipeVolume { get; set; } 
    public string ProductRecipeVolumeUnit { get; set; } = default!;
    
    /// <summary>
    /// ID of the component product used in the recipe.
    /// Name of the component product.
    /// Product code (SKU or internal identifier).
    /// Unit of measurement used for the  component product (e.g., "g", "ml").
    /// Total volume of the  component product.
    /// Measurement unit for the Volume field (e.g., "l", "ml", "g", "kg").
    /// </summary>
    public Guid ComponentProductId { get; set; }
    public string ComponentProductName { get; set; } = default!;
    public string ComponentProductCode { get; set; } = default!;
    public string ComponentProductUnit { get; set; } = default!;
    public decimal ComponentProductVolume { get; set; } 
    public string ComponentProductVolumeUnit { get; set; } = default!;

    /// <summary>
    /// Quantity of the component required to produce one unit of the recipe product.
    /// </summary>
    public decimal Amount { get; set; }
}