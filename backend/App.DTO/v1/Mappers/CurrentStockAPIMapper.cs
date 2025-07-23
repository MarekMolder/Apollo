using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class CurrentStockAPIMapper : IMapper<App.DTO.v1.CurrentStock, App.BLL.DTO.CurrentStock>
{
    public App.DTO.v1.CurrentStock? Map(App.BLL.DTO.CurrentStock? entity)
    {
        if (entity == null) return null;
        var res = new App.DTO.v1.CurrentStock()
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            UpdatedAt = entity.UpdatedAt,
            ProductId = entity.ProductId,
            StorageRoomId = entity.StorageRoomId,
        };
        return res;
    }

    public App.BLL.DTO.CurrentStock? Map(App.DTO.v1.CurrentStock? entity)
    {
        if (entity == null) return null;
        var res = new App.BLL.DTO.CurrentStock()
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            UpdatedAt = entity.UpdatedAt,
            ProductId = entity.ProductId,
            StorageRoomId = entity.StorageRoomId,
        };
        return res;
    }
    
    public App.BLL.DTO.CurrentStock Map(App.DTO.v1.CurrentStockCreate entity)
    {
        var res = new App.BLL.DTO.CurrentStock()
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