using Base.Domain;

namespace App.Domain.Logic;

/// <summary>
/// Represents a category to which products belong (e.g., Beverages, Snacks, Cleaning Supplies).
/// </summary>
public class ProductCategory : BaseEntity
{
    /// <summary>
    /// Name of the category (e.g., "Drinks", "Food").
    /// </summary>
    public string Name { get; set; } = default!;
    
    /// <summary>
    /// Optional end date indicating when the category became inactive.
    /// </summary
    public DateTime? EndedAt { get; set; }
    
    /// <summary>
    /// Collection of products that belong to this category.
    /// </summary>
    public ICollection<Domain.Logic.Product>? Products { get; set; }
}