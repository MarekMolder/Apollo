using Base.Contracts;

namespace App.DAL.DTO;

/// <summary>
/// Represents aggregated monthly statistics of removed quantities for a specific product in a storage room.
/// </summary>
public class MonthlyStatistics : IDomainId
{
    /// <summary>
    /// Unique identifier of the statistics record.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// ID of the product for which the statistics apply.
    /// Related product entity.
    /// </summary>
    public Guid ProductId { get; set; }
    public DAL.DTO.Product? Product { get; set; }
    
    /// <summary>
    /// ID of the productCategory for which the statistics apply.
    /// Related productCategory entity.
    /// </summary>
    public Guid ProductCategoryId { get; set; }
    public DAL.DTO.ProductCategory? ProductCategory { get; set; }
    
    /// <summary>
    /// Total quantity of the product removed during the specified month.
    /// </summary>
    public decimal TotalRemovedQuantity { get; set; }
    
    /// <summary>
    /// ID of the storage room where the product was removed from.
    /// Related storage room entity.
    /// </summary>
    public Guid StorageRoomId { get; set; }
    public DAL.DTO.StorageRoom? StorageRoom { get; set; }
    
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