using App.DTO.v1.ApiEntities;
using Base.Contracts;

namespace App.DTO.v1.ApiMapper;

public class EnrichedMonthlyStatisticsAPIMapper : IMapper<EnrichedMonthlyStatistics, BLL.DTO.MonthlyStatistics>
{
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