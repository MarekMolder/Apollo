namespace App.DTO;

/// <summary>
/// Represents aggregated monthly statistics of removed quantities for a specific product in a storage room.
/// </summary>
public class MonthlyStatisticsCreate
{
    /// <summary>
    /// ID of the product for which the statistics apply.
    /// </summary>
    public Guid ProductId { get; set; }
    
    /// <summary>
    /// ID of the storage room where the product was removed from.
    /// </summary>
    public Guid StorageRoomId { get; set; }
    
    /// <summary>
    /// Total quantity of the product removed during the specified month.
    /// </summary>
    public decimal TotalRemovedQuantity { get; set; }
    
    /// <summary>
    /// Year of the statistics period.
    /// </summary>
    public int Year { get; set; }
    
    /// <summary>
    /// Month of the statistics period (1 = January, 12 = December).
    /// </summary>
    public int Month { get; set; }
    
    /// <summary>
    /// Start date of the period (always first day of the month).
    /// </summary>
    public DateTime PeriodStart => new DateTime(Year, Month, 1);
}