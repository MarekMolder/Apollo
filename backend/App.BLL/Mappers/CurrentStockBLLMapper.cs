using App.DAL.DTO;
using Base.Contracts;

namespace App.BLL.Mappers;

public class CurrentStockBLLMapper : IMapper<App.BLL.DTO.CurrentStock, CurrentStock>
{
    public CurrentStock? Map(DTO.CurrentStock? entity)
    {
        if (entity == null) return null;
        
        var res = new CurrentStock()
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            UpdatedAt = entity.UpdatedAt,
            
            ProductId = entity.ProductId,
            Product = ProductBLLMapper.MapSimple(entity.Product),
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomBLLMapper.MapSimple(entity.StorageRoom),
        };
        return res;
    }

    public DTO.CurrentStock? Map(CurrentStock? entity)
    {
        if (entity == null) return null;
        
        var res = new DTO.CurrentStock()
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            UpdatedAt = entity.UpdatedAt,
            
            ProductId = entity.ProductId,
            Product = ProductBLLMapper.MapSimple(entity.Product),
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomBLLMapper.MapSimple(entity.StorageRoom),
        };
        return res;
    }
}