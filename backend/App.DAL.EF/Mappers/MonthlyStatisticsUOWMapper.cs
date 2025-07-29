using App.DAL.DTO;
using Base.Contracts;

namespace App.DAL.EF.Mappers;

public class MonthlyStatisticsUOWMapper: IMapper<MonthlyStatistics, Domain.Logic.MonthlyStatistics>
{    
    public MonthlyStatistics? Map(Domain.Logic.MonthlyStatistics? entity)
    {
        if (entity == null) return null;
        
        var res = new MonthlyStatistics
        {
            Id = entity.Id,
            
            ProductId = entity.ProductId,
            Product = ProductUOWMapper.MapSimple(entity.Product),
            
            TotalRemovedQuantity = entity.TotalRemovedQuantity,
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomUOWMapper.MapSimple(entity.StorageRoom),
            
            Year = entity.Year,
            Month = entity.Month,
        };
        return res;
    }

    public Domain.Logic.MonthlyStatistics? Map(MonthlyStatistics? entity)
    {
        if (entity == null) return null;
        
        var res = new Domain.Logic.MonthlyStatistics
        {
            Id = entity.Id,
            
            ProductId = entity.ProductId,
            Product = ProductUOWMapper.MapSimple(entity.Product),
            
            TotalRemovedQuantity = entity.TotalRemovedQuantity,
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomUOWMapper.MapSimple(entity.StorageRoom),
            
            Year = entity.Year,
            Month = entity.Month,
        };
        return res;
    }
}
