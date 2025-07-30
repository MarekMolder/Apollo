
namespace App.DTO.v1;

public class ProductCreate
{
    /// <summary>
    /// Unit of measurement (e.g., "g", "ml", "L").
    /// </summary>
    public string Unit { get; set; } = default!;
    
    /// <summary>
    /// Volume or weight of the product per unit (e.g., 0.5 for liter bottle).
    /// </summary>
    public decimal Volume { get; set; } = default!;

    /// <summary>
    /// Internal or external product code.
    /// </summary>
    public string Code { get; set; } = default!;
    
    /// <summary>
    /// Human-readable product name.
    /// </summary>
    public string Name { get; set; } = default!;
    
    /// <summary>
    /// Price of one unit of the product.
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Unit of the product.
    /// </summary>
    public decimal Quantity { get; set; }
    
    /// <summary>
    /// Foreign key for product category.
    /// </summary>
    public Guid ProductCategoryId { get; set; }
    
    /// <summary>
    /// Indicates if the product is a component in a recipe.
    /// </summary>
    public bool IsComponent { get; set; } = false;
}