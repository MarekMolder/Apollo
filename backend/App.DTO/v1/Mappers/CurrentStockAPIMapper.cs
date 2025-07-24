using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class CurrentStockAPIMapper : IMapper<CurrentStock, BLL.DTO.CurrentStock>
{
    public CurrentStock? Map(BLL.DTO.CurrentStock? entity)
    {
        if (entity == null) return null;
        var res = new CurrentStock
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            UpdatedAt = entity.UpdatedAt,
            ProductId = entity.ProductId,
            StorageRoomId = entity.StorageRoomId,
        };
        return res;
    }

    public BLL.DTO.CurrentStock? Map(CurrentStock? entity)
    {
        if (entity == null) return null;
        var res = new BLL.DTO.CurrentStock
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            UpdatedAt = entity.UpdatedAt,
            ProductId = entity.ProductId,
            StorageRoomId = entity.StorageRoomId,
        };
        return res;
    }
    
    public BLL.DTO.CurrentStock Map(CurrentStockCreate entity)
    {
        var res = new BLL.DTO.CurrentStock
        {
            Id = Guid.NewGuid(),
            Quantity = entity.Quantity,
            UpdatedAt = entity.UpdatedAt,
            ProductId = entity.ProductId,
            StorageRoomId = entity.StorageRoomId,
        };
        return res;
    }
}