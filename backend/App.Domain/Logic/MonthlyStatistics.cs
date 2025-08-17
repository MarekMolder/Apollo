using Base.Domain;

namespace App.Domain.Logic;

/// <summary>
/// Represents aggregated monthly statistics of removed quantities for a specific product in a storage room.
/// </summary>
public class MonthlyStatistics : BaseEntity
{
    /// <summary>
    /// ID of the product for which the statistics apply.
    /// Related product entity.
    /// </summary>
    public Guid ProductId { get; set; }
    public Domain.Logic.Product? Product { get; set; }
    
    /// <summary>
    /// ID of the productCategory for which the statistics apply.
    /// Related productCategory entity.
    /// </summary>
    public Guid ProductCategoryId { get; set; }
    public Domain.Logic.ProductCategory? ProductCategory { get; set; }
    
    /// <summary>
    /// Total quantity of the product removed during the specified month.
    /// </summary>
    public decimal TotalRemovedQuantity { get; set; }
    
    /// <summary>
    /// ID of the storage room where the product was removed from.
    /// Related storage room entity.
    /// </summary>
    public Guid StorageRoomId { get; set; }
    public Domain.Logic.StorageRoom? StorageRoom { get; set; }
    
    /// <summary>
    /// Year of the statistics period.
    /// </summary>
    public int Year { get; set; }
    
    /// <summary>
    /// Month of the statistics period (1 = January, 12 = December).
    /// </summary>
    public int Month { get; set; }
    
    /// <summary>
    /// Day of the statistics period.
    /// </summary>
    public int Day { get; set; }
    
    /// <summary>
    /// Start date of the period (always first day of the month).
    /// </summary>
    public DateTime PeriodStart => new DateTime(Year, Month, 1);
}