using Base.Contracts;

namespace App.DAL.EF.Mappers;

/// <summary>
/// Maps between Domain and DAL DTO MonthlyStatistics entities.
/// Handles both full mapping and simplified mapping.
/// </summary>
public class MonthlyStatisticsUowMapper: IMapper<DAL.DTO.MonthlyStatistics, Domain.Logic.MonthlyStatistics>
{    
    /// <summary>
    /// Maps a full Domain MonthlyStatistics entity to a DAL DTO Address MonthlyStatistics, including related StorageRooms and Products.
    /// </summary>
    public DAL.DTO.MonthlyStatistics? Map(Domain.Logic.MonthlyStatistics? entity)
    {
        if (entity == null) return null;
        
        var res = new DAL.DTO.MonthlyStatistics
        {
            Id = entity.Id,
            
            ProductId = entity.ProductId,
            Product = ProductUowMapper.MapSimple(entity.Product),
            
            TotalRemovedQuantity = entity.TotalRemovedQuantity,
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomUowMapper.MapSimple(entity.StorageRoom),
            
            Year = entity.Year,
            Month = entity.Month,
        };
        return res;
    }

    /// <summary>
    /// Maps a full DAL DTO MonthlyStatistics entity to a Domain MonthlyStatistics entity, including related StorageRooms and Products.
    /// </summary>
    public Domain.Logic.MonthlyStatistics? Map(DAL.DTO.MonthlyStatistics? entity)
    {
        if (entity == null) return null;
        
        var res = new Domain.Logic.MonthlyStatistics
        {
            Id = entity.Id,
            
            ProductId = entity.ProductId,
            Product = ProductUowMapper.MapSimple(entity.Product),
            
            TotalRemovedQuantity = entity.TotalRemovedQuantity,
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomUowMapper.MapSimple(entity.StorageRoom),
            
            Year = entity.Year,
            Month = entity.Month,
        };
        return res;
    }
}
