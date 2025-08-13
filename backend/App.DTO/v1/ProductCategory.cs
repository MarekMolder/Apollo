using Base.Contracts;

namespace App.DTO.v1;

/// <summary>
/// Represents a category to which products belong (e.g., Beverages, Snacks, Cleaning Supplies).
/// </summary>
public class ProductCategory : IDomainId
{
    /// <summary>
    /// Unique identifier of the product category.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Name of the category (e.g., "Drinks", "Food").
    /// </summary>
    public string Name { get; set; } = default!;
}