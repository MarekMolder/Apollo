using Base.Contracts;

namespace App.DTO.v1.ApiEntities;

/// <summary>
/// Data Transfer Object (DTO) representing an enriched view of an MonthlyStatistics.
/// Includes related product and storage room names for simplified API consumption.
/// </summary>
public class EnrichedMonthlyStatistics : IDomainId
{
    /// <summary>
    /// Unique identifier of the statistics record.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// ID of the product for which the statistics apply.
    /// Name of the product.
    /// Product code (SKU or internal identifier).
    /// Unit of measurement used for the product (e.g., "g", "ml").
    /// Total volume of the product removed during the specified month.
    /// Measurement unit for the Volume field (e.g., "l", "ml", "g", "kg").
    /// </summary>
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = default!;
    public string ProductCode { get; set; } = default!;
    public string ProductUnit { get; set; } = default!;
    public decimal ProductVolume { get; set; } 
    public string ProductVolumeUnit { get; set; } = default!;
    
    /// <summary>
    /// ID of the storage room where the product was removed from.
    /// Name of the storage room.
    /// </summary>
    public Guid StorageRoomId { get; set; }
    public string StorageRoomName { get; set; } = default!;
    
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