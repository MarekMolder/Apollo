
namespace App.DTO.v1;

/// <summary>
/// Represents a category to which products belong (e.g., Beverages, Snacks, Cleaning Supplies).
/// </summary>
public class ProductCategoryCreate
{
    /// <summary>
    /// Name of the category (e.g., "Drinks", "Food").
    /// </summary>
    public string Name { get; set; } = default!;
    
    /// <summary>
    /// Optional end date indicating when the category became inactive.
    /// </summary
    public DateTime? EndedAt { get; set; }
}