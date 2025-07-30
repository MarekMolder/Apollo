using App.DTO.v1.ApiEntities;
using Base.Contracts;

namespace App.DTO.v1.ApiMapper;

/// <summary>
/// Maps between DTO V1 and BLL DTO MonthlyStatistics objects.
/// Includes both full and simplified mapping logic.
/// </summary>
public class EnrichedMonthlyStatisticsApiMapper : IMapper<EnrichedMonthlyStatistics, BLL.DTO.MonthlyStatistics>
{
    /// <summary>
    /// Maps a full BLL DTO MonthlyStatistics to a DTO V1 MonthlyStatistics, including related objects.
    /// </summary>
    public EnrichedMonthlyStatistics? Map(BLL.DTO.MonthlyStatistics? entity)
    {
        if (entity == null) return null;

        return new EnrichedMonthlyStatistics
        {
            Id = entity.Id,
            
            ProductId = entity.ProductId,
            ProductCode = entity.Product?.Code ?? "Unknown",
            ProductName = entity.Product?.Name ?? "Unknown",
            ProductUnit = entity.Product?.Unit ?? "Unknown",
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoomName = entity.StorageRoom?.Name ?? "Unknown",
            
            TotalRemovedQuantity = entity.TotalRemovedQuantity,
            Year = entity.Year,
            Month = entity.Month,
        };
    }

    public BLL.DTO.MonthlyStatistics? Map(EnrichedMonthlyStatistics? entity)
    {
        throw new NotImplementedException("Mapping from API to BLL is not required.");
    }
}