using App.DAL.DTO;
using Base.Contracts;
using Base.DAL.Contracts;

namespace App.DAL.EF.Mappers;

public class StockMovementUOWMapper: IMapper<App.DAL.DTO.StockMovement, App.Domain.Logic.StockMovement>
{
    public StockMovement? Map(Domain.Logic.StockMovement? entity)
    {
        if (entity == null) return null;
        
        var res = new StockMovement()
        {
            Id = entity.Id,
            Amount = entity.Amount,
            
            ProductId = entity.ProductId,
            Product = ProductUOWMapper.MapSimple(entity.Product),
            
            FromStorageRoomId = entity.FromStorageRoomId,
            FromStorageRoom = StorageRoomUOWMapper.MapSimple(entity.FromStorageRoom),
            
            ToStorageRoomId = entity.ToStorageRoomId,
            ToStorageRoom = StorageRoomUOWMapper.MapSimple(entity.ToStorageRoom),
            
            FromInventoryId = entity.FromInventoryId,
            FromInventory = InventoryUOWMapper.MapSimple(entity.FromInventory),
            
            ToInventoryId = entity.ToInventoryId,
            ToInventory = InventoryUOWMapper.MapSimple(entity.ToInventory),
            
        };
        return res;
    }

    public Domain.Logic.StockMovement? Map(StockMovement? entity)
    {
        if (entity == null) return null;
        
        var res = new Domain.Logic.StockMovement()
        {
            Id = entity.Id,
            Amount = entity.Amount,
            
            ProductId = entity.ProductId,
            Product = ProductUOWMapper.MapSimple(entity.Product),
            
            FromStorageRoomId = entity.FromStorageRoomId,
            FromStorageRoom = StorageRoomUOWMapper.MapSimple(entity.FromStorageRoom),
            
            ToStorageRoomId = entity.ToStorageRoomId,
            ToStorageRoom = StorageRoomUOWMapper.MapSimple(entity.ToStorageRoom),
            
            FromInventoryId = entity.FromInventoryId,
            FromInventory = InventoryUOWMapper.MapSimple(entity.FromInventory),
            
            ToInventoryId = entity.ToInventoryId,
            ToInventory = InventoryUOWMapper.MapSimple(entity.ToInventory),
            
        };
        return res;
    }
    public static StockMovement? MapSimple(Domain.Logic.StockMovement? entity)
    {
        if (entity == null) return null;

        return new StockMovement()
        {
            Id = entity.Id,
            Amount = entity.Amount,
            FromStorageRoomId = entity.FromStorageRoomId,
            ToStorageRoomId = entity.ToStorageRoomId,
            FromInventoryId = entity.FromInventoryId,
            ToInventoryId = entity.ToInventoryId,
        };
    }

    public static Domain.Logic.StockMovement? MapSimple(StockMovement? entity)
    {
        if (entity == null) return null;

        return new Domain.Logic.StockMovement()
        {
            Id = entity.Id,
            Amount = entity.Amount,
            FromStorageRoomId = entity.FromStorageRoomId,
            ToStorageRoomId = entity.ToStorageRoomId,
            FromInventoryId = entity.FromInventoryId,
            ToInventoryId = entity.ToInventoryId,
        };
    }
}
