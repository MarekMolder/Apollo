using App.DAL.DTO;
using Base.BLL.Contracts;
using Base.Contracts;

namespace App.BLL.Mappers;

public class StockMovementBLLMapper : IMapper<App.BLL.DTO.StockMovement, App.DAL.DTO.StockMovement>
{
    public StockMovement? Map(DTO.StockMovement? entity)
    {
        if (entity == null) return null;
        
        var res = new StockMovement()
        {
            Id = entity.Id,
            Amount = entity.Amount,
            
            ProductId = entity.ProductId,
            Product = ProductBLLMapper.MapSimple(entity.Product),
            
            FromStorageRoomId = entity.FromStorageRoomId,
            FromStorageRoom = StorageRoomBLLMapper.MapSimple(entity.FromStorageRoom),
            
            ToStorageRoomId = entity.ToStorageRoomId,
            ToStorageRoom = StorageRoomBLLMapper.MapSimple(entity.ToStorageRoom),
            
            FromInventoryId = entity.FromInventoryId,
            FromInventory = InventoryBLLMapper.MapSimple(entity.FromInventory),
            
            ToInventoryId = entity.ToInventoryId,
            ToInventory = InventoryBLLMapper.MapSimple(entity.ToInventory),
            
        };
        return res;
    }

    public DTO.StockMovement? Map(StockMovement? entity)
    {
        if (entity == null) return null;
        
        var res = new DTO.StockMovement()
        {
            Id = entity.Id,
            Amount = entity.Amount,
            
            FromStorageRoomId = entity.FromStorageRoomId,
            FromStorageRoom = StorageRoomBLLMapper.MapSimple(entity.FromStorageRoom),
            
            ToStorageRoomId = entity.ToStorageRoomId,
            ToStorageRoom = StorageRoomBLLMapper.MapSimple(entity.ToStorageRoom),
            
            FromInventoryId = entity.FromInventoryId,
            FromInventory = InventoryBLLMapper.MapSimple(entity.FromInventory),
            
            ToInventoryId = entity.ToInventoryId,
            ToInventory = InventoryBLLMapper.MapSimple(entity.ToInventory),
            
        };
        return res;
    }
    
    public static StockMovement? MapSimple(DTO.StockMovement? entity)
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
    
    public static DTO.StockMovement? MapSimple(StockMovement? entity)
    {
        if (entity == null) return null;

        return new DTO.StockMovement()
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