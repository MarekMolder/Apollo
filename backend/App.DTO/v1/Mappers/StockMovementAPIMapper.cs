using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class StockMovementAPIMapper : IMapper<App.DTO.v1.StockMovement, App.BLL.DTO.StockMovement>
{
    public App.DTO.v1.StockMovement? Map(App.BLL.DTO.StockMovement? entity)
    {
        if (entity == null) return null;
        var res = new App.DTO.v1.StockMovement()
        {
            Id = entity.Id,
            Amount = entity.Amount,
            ProductId = entity.ProductId,
            FromStorageRoomId = entity.FromStorageRoomId,
            ToStorageRoomId = entity.ToStorageRoomId,
            FromInventoryId = entity.FromInventoryId,
            ToInventoryId = entity.ToInventoryId,
        };
        return res;
    }

    public App.BLL.DTO.StockMovement? Map(App.DTO.v1.StockMovement? entity)
    {
        if (entity == null) return null;
        var res = new App.BLL.DTO.StockMovement()
        {
            Id = entity.Id,
            Amount = entity.Amount,
            ProductId = entity.ProductId,
            FromStorageRoomId = entity.FromStorageRoomId,
            ToStorageRoomId = entity.ToStorageRoomId,
            FromInventoryId = entity.FromInventoryId,
            ToInventoryId = entity.ToInventoryId,
        };
        return res;
    }
    
    public App.BLL.DTO.StockMovement Map(App.DTO.v1.StockMovementCreate entity)
    {
        var res = new App.BLL.DTO.StockMovement()
        {
            Id = Guid.NewGuid(),
            Amount = entity.Amount,
            ProductId = entity.ProductId,
            FromStorageRoomId = entity.FromStorageRoomId,
            ToStorageRoomId = entity.ToStorageRoomId,
            FromInventoryId = entity.FromInventoryId,
            ToInventoryId = entity.ToInventoryId,
        };
        return res;
    }
}