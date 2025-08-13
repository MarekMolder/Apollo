using Base.Contracts;

namespace App.DTO.v1;

/// <summary>
/// Represents a product in the inventory system, which can be a standalone item or a component of a recipe.
/// </summary>
public class Product : IDomainId
{
    /// <summary>
    /// Unique identifier of the product.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Unit of measurement (e.g., "g", "ml", "L").
    /// </summary>
    public string Unit { get; set; } = default!;
    
    /// <summary>
    /// Volume or weight of the product per unit (e.g., 0.5 for liter bottle).
    /// </summary>
    public decimal Volume { get; set; } = default!;
    
    /// <summary>
    /// Measurement unit for the Volume field (e.g., "l", "ml", "g", "kg").
    /// This allows distinguishing volume when Unit is "tk".
    /// </summary>
    public string VolumeUnit { get; set; } = default!;

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
    
    /// <summary>
    /// Foreign key to the supplier who provides this product.
    /// </summary>
    public Guid? SupplierId { get; set; }
}