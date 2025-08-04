using Base.Contracts;

namespace App.BLL.Mappers;

/// <summary>
/// Maps between BLL DTO and DAL DTO MonthlyStatistics entities.
/// Handles both full mapping and simplified mapping.
/// </summary>
public class MonthlyStatisticsBllMapper: IMapper<BLL.DTO.MonthlyStatistics, DAL.DTO.MonthlyStatistics>
{
    /// <summary>
    /// Maps a full BLL DTO MonthlyStatistics entity to a DAL DTO Address MonthlyStatistics, including related StorageRooms and Products.
    /// </summary>
    public DAL.DTO.MonthlyStatistics? Map(BLL.DTO.MonthlyStatistics? entity)
    {
        if (entity == null) return null;
        
        var res = new DAL.DTO.MonthlyStatistics()
        {
            Id = entity.Id,
            
            ProductId = entity.ProductId,
            Product = ProductBllMapper.MapSimple(entity.Product),
            
            TotalRemovedQuantity = entity.TotalRemovedQuantity,
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomBllMapper.MapSimple(entity.StorageRoom),
            
            Year = entity.Year,
            Month = entity.Month,
        };
        return res;
    }
    
    /// <summary>
    /// Maps a full DAL DTO MonthlyStatistics entity to a BLL DTO MonthlyStatistics entity, including related StorageRooms and Products.
    /// </summary>
    public BLL.DTO.MonthlyStatistics? Map(DAL.DTO.MonthlyStatistics? entity)
    {
        if (entity == null) return null;
        
        var res = new BLL.DTO.MonthlyStatistics()
        {
            Id = entity.Id,
            
            ProductId = entity.ProductId,
            Product = ProductBllMapper.MapSimple(entity.Product),
            
            TotalRemovedQuantity = entity.TotalRemovedQuantity,
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomBllMapper.MapSimple(entity.StorageRoom),
            
            Year = entity.Year,
            Month = entity.Month,
        };
        return res;
    }
}
