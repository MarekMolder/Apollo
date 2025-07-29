namespace App.DTO;

public class MonthlyStatisticsCreate
{
    public Guid ProductId { get; set; }
    public Guid StorageRoomId { get; set; }
    
    public decimal TotalRemovedQuantity { get; set; }
    
    public int Year { get; set; }
    public int Month { get; set; }
    public DateTime PeriodStart => new DateTime(Year, Month, 1);
}