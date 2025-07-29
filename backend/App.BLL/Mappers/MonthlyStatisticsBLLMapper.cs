using App.DAL.DTO;
using Base.Contracts;

namespace App.BLL.Mappers;

public class MonthlyStatisticsBLLMapper: IMapper<App.BLL.DTO.MonthlyStatistics, MonthlyStatistics>
{
    public MonthlyStatistics? Map(DTO.MonthlyStatistics? entity)
    {
        if (entity == null) return null;
        
        var res = new MonthlyStatistics()
        {
            Id = entity.Id,
            
            ProductId = entity.ProductId,
            Product = ProductBLLMapper.MapSimple(entity.Product),
            
            TotalRemovedQuantity = entity.TotalRemovedQuantity,
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomBLLMapper.MapSimple(entity.StorageRoom),
            
            Year = entity.Year,
            Month = entity.Month,
        };
        return res;
    }

    public DTO.MonthlyStatistics? Map(MonthlyStatistics? entity)
    {
        if (entity == null) return null;
        
        var res = new DTO.MonthlyStatistics()
        {
            Id = entity.Id,
            
            ProductId = entity.ProductId,
            Product = ProductBLLMapper.MapSimple(entity.Product),
            
            TotalRemovedQuantity = entity.TotalRemovedQuantity,
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomBLLMapper.MapSimple(entity.StorageRoom),
            
            Year = entity.Year,
            Month = entity.Month,
        };
        return res;
    }
}