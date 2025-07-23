using App.DAL.DTO;
using Base.BLL.Contracts;
using Base.Contracts;
using ActionTypeEntity = App.BLL.DTO.ActionTypeEntity;

namespace App.BLL.Mappers;

public class ActionEntityBLLMapper : IMapper<App.BLL.DTO.ActionEntity, App.DAL.DTO.ActionEntity>
{
    
    public ActionEntity? Map(DTO.ActionEntity? entity)
    {
        if (entity == null) return null;

        var res = new ActionEntity()
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            Status = entity.Status,
            
            ActionTypeId = entity.ActionTypeId,
            ActionType = ActionTypeEntityBLLMapper.MapSimple(entity.ActionType),
            
            ReasonId = entity.ReasonId,
            Reason = ReasonBLLMapper.MapSimple(entity.Reason),
            
            SupplierId = entity.SupplierId,
            Supplier = SupplierBLLMapper.MapSimple(entity.Supplier),
            
            ProductId = entity.ProductId,
            Product = ProductBLLMapper.MapSimple(entity.Product),
            
            StockAuditId = entity.StockAuditId,
            StockAudit = StockAuditBLLMapper.MapSimple(entity.StockAudit),
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomBLLMapper.MapSimple(entity.StorageRoom),
            
        };
        return res;
    }

    public DTO.ActionEntity? Map(ActionEntity? entity)
    {
        if (entity == null) return null;

        var res = new DTO.ActionEntity()
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            Status = entity.Status,
            
            ActionTypeId = entity.ActionTypeId,
            ActionType = ActionTypeEntityBLLMapper.MapSimple(entity.ActionType),
            
            ReasonId = entity.ReasonId,
            Reason = ReasonBLLMapper.MapSimple(entity.Reason),
            
            SupplierId = entity.SupplierId,
            Supplier = SupplierBLLMapper.MapSimple(entity.Supplier),
            
            ProductId = entity.ProductId,
            Product = ProductBLLMapper.MapSimple(entity.Product),
            
            StockAuditId = entity.StockAuditId,
            StockAudit = StockAuditBLLMapper.MapSimple(entity.StockAudit),
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomBLLMapper.MapSimple(entity.StorageRoom),
        };
        return res;
    }
    
    public static ActionEntity? MapSimple(DTO.ActionEntity? entity)
    {
        if (entity == null) return null;

        return new ActionEntity()
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            Status = entity.Status,
            ActionTypeId = entity.ActionTypeId,
            ReasonId = entity.ReasonId,
            SupplierId = entity.SupplierId,
            ProductId = entity.ProductId,
            StockAuditId = entity.StockAuditId,
            StorageRoomId = entity.StorageRoomId,
        };
    }
    
    public static DTO.ActionEntity? MapSimple(ActionEntity? entity)
    {
        if (entity == null) return null;

        return new DTO.ActionEntity()
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            Status = entity.Status,
            ActionTypeId = entity.ActionTypeId,
            ReasonId = entity.ReasonId,
            SupplierId = entity.SupplierId,
            ProductId = entity.ProductId,
            StockAuditId = entity.StockAuditId,
            StorageRoomId = entity.StorageRoomId,
        };
    }
}