using Base.Contracts;

namespace App.DTO.v1.ApiEntities;

/// <summary>
/// Data Transfer Object (DTO) representing an enriched view of a Product entity.
/// Includes product category name for simplified API consumption.
/// </summary>
public class EnrichedProduct : IDomainId
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
    /// Name of the product category.
    /// </summary>
    public Guid ProductCategoryId { get; set; }
    public string ProductCategoryName { get; set; } = default!;
    
    /// <summary>
    /// Foreign key for Supplier.
    /// Name of the product category.
    /// </summary>
    public Guid? SupplierId { get; set; }
    public string SupplierName { get; set; } = default!;
    public string SupplierEmail { get; set; } = default!;
    
    /// <summary>
    /// Indicates if the product is a component in a recipe.
    /// </summary>
    public bool IsComponent { get; set; } = false;
}