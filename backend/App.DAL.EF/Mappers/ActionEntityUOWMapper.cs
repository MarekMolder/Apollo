using App.DAL.DTO;
using Base.Contracts;

namespace App.DAL.EF.Mappers;

public class ActionEntityUOWMapper : IMapper<ActionEntity, Domain.Logic.ActionEntity>
{
    public ActionEntity? Map(Domain.Logic.ActionEntity? entity)
    {
        if (entity == null) return null;

        var res = new ActionEntity
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            Status = entity.Status,
            
            ActionTypeId = entity.ActionTypeId,
            ActionType = ActionTypeEntityUOWMapper.MapSimple(entity.ActionType),
            
            ReasonId = entity.ReasonId,
            Reason = ReasonUOWMapper.MapSimple(entity.Reason),
            
            SupplierId = entity.SupplierId,
            Supplier = SupplierUOWMapper.MapSimple(entity.Supplier),
            
            ProductId = entity.ProductId,
            Product = ProductUOWMapper.MapSimple(entity.Product),
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomUOWMapper.MapSimple(entity.StorageRoom),
            
        };
        return res;
    }

    public Domain.Logic.ActionEntity? Map(ActionEntity? entity)
    {
        if (entity == null) return null;

        var res = new Domain.Logic.ActionEntity
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            Status = entity.Status,
            
            ActionTypeId = entity.ActionTypeId,
            ActionType = ActionTypeEntityUOWMapper.MapSimple(entity.ActionType),
            
            ReasonId = entity.ReasonId,
            Reason = ReasonUOWMapper.MapSimple(entity.Reason),
            
            SupplierId = entity.SupplierId,
            Supplier = SupplierUOWMapper.MapSimple(entity.Supplier),
            
            ProductId = entity.ProductId,
            Product = ProductUOWMapper.MapSimple(entity.Product),
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomUOWMapper.MapSimple(entity.StorageRoom),
        };
        return res;
    }
    
    public static ActionEntity? MapSimple(Domain.Logic.ActionEntity? entity)
    {
        if (entity == null) return null;

        return new ActionEntity
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            Status = entity.Status,
            ActionTypeId = entity.ActionTypeId,
            ReasonId = entity.ReasonId,
            SupplierId = entity.SupplierId,
            ProductId = entity.ProductId,
            StorageRoomId = entity.StorageRoomId,
        };
    }
    
    public static Domain.Logic.ActionEntity? MapSimple(ActionEntity? entity)
    {
        if (entity == null) return null;

        return new Domain.Logic.ActionEntity
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            Status = entity.Status,
            ActionTypeId = entity.ActionTypeId,
            ReasonId = entity.ReasonId,
            SupplierId = entity.SupplierId,
            ProductId = entity.ProductId,
            StorageRoomId = entity.StorageRoomId,
        };
    }
}
