using Base.Contracts;

namespace App.DTO.v1.ApiEntities;

public class EnrichedMonthlyStatistics : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = default!;
    public string ProductCode { get; set; } = default!;
    public string ProductUnit { get; set; } = default!;
    
    public Guid StorageRoomId { get; set; }
    public string StorageRoomName { get; set; } = default!;
    
    public decimal TotalRemovedQuantity { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public DateTime PeriodStart => new DateTime(Year, Month, 1);
    
    
    

}