using App.DAL.DTO;
using Base.Contracts;
using Base.DAL.Contracts;

namespace App.DAL.EF.Mappers;

public class CurrentStockUOWMapper: IMapper<App.DAL.DTO.CurrentStock, App.Domain.Logic.CurrentStock>
{    
    public CurrentStock? Map(Domain.Logic.CurrentStock? entity)
    {
        if (entity == null) return null;
        
        var res = new CurrentStock()
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            UpdatedAt = entity.UpdatedAt,
            
            ProductId = entity.ProductId,
            Product = ProductUOWMapper.MapSimple(entity.Product),
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomUOWMapper.MapSimple(entity.StorageRoom),
        };
        return res;
    }

    public Domain.Logic.CurrentStock? Map(CurrentStock? entity)
    {
        if (entity == null) return null;
        
        var res = new Domain.Logic.CurrentStock()
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            UpdatedAt = entity.UpdatedAt,
            
            ProductId = entity.ProductId,
            Product = ProductUOWMapper.MapSimple(entity.Product),
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomUOWMapper.MapSimple(entity.StorageRoom),
        };
        return res;
    }
}
