using Base.Contracts;

namespace App.DTO.v1.Mappers;

/// <summary>
/// Maps between DTO V1 and BLL DTO MonthlyStatistics objects.
/// Includes both full and simplified mapping logic.
/// </summary>
public class MonthlyStatisticsApiMapper : IMapper<MonthlyStatistics, BLL.DTO.MonthlyStatistics>
{
    /// <summary>
    /// Maps a simplified BLL DTO MonthlyStatistics to DTO V1 MonthlyStatistics, without related objects.
    /// </summary>
    public MonthlyStatistics? Map(BLL.DTO.MonthlyStatistics? entity)
    {
        if (entity == null) return null;
        var res = new MonthlyStatistics
        {
            Id = entity.Id,
            ProductId = entity.ProductId,
            StorageRoomId = entity.StorageRoomId,
            TotalRemovedQuantity = entity.TotalRemovedQuantity,
            Year = entity.Year,
            Month = entity.Month,
        };
        return res;
    }

    /// <summary>
    /// Maps a simplified DTO V1 MonthlyStatistics to BLL DTO MonthlyStatistics, without related objects.
    /// </summary>
    public BLL.DTO.MonthlyStatistics? Map(MonthlyStatistics? entity)
    {
        if (entity == null) return null;
        var res = new BLL.DTO.MonthlyStatistics
        {
            Id = entity.Id,
            ProductId = entity.ProductId,
            StorageRoomId = entity.StorageRoomId,
            TotalRemovedQuantity = entity.TotalRemovedQuantity,
            Year = entity.Year,
            Month = entity.Month,
        };
        return res;
    }
    
    /// <summary>
    /// Maps a simplified DTO V1 MonthlyStatisticsCreate to BLL DTO MonthlyStatistics, without related objects.
    /// </summary>
    public BLL.DTO.MonthlyStatistics Map(MonthlyStatisticsCreate entity)
    {
        var res = new BLL.DTO.MonthlyStatistics
        {
            Id = Guid.NewGuid(),
            ProductId = entity.ProductId,
            StorageRoomId = entity.StorageRoomId,
            TotalRemovedQuantity = entity.TotalRemovedQuantity,
            Year = entity.Year,
            Month = entity.Month,
        };
        return res;
    }
}