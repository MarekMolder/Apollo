using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class MonthlyStatisticsAPIMapper : IMapper<MonthlyStatistics, BLL.DTO.MonthlyStatistics>
{
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