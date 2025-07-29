using Base.Contracts;

namespace App.BLL.DTO;

public class MonthlyStatistics : IDomainId
{
    public Guid Id { get; set; }
    
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    
    public decimal TotalRemovedQuantity { get; set; }
    
    public Guid StorageRoomId { get; set; }
    public StorageRoom? StorageRoom { get; set; }
    
    public int Year { get; set; }
    public int Month { get; set; }
    public DateTime PeriodStart => new DateTime(Year, Month, 1);
}