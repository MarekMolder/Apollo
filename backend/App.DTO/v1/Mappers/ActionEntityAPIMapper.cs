using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class ActionEntityAPIMapper : IMapper<App.DTO.v1.ActionEntity, App.BLL.DTO.ActionEntity>
{
    public App.DTO.v1.ActionEntity? Map(App.BLL.DTO.ActionEntity? entity)
    {
        if (entity == null) return null;
        var res = new App.DTO.v1.ActionEntity()
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
        return res;
    }

    public App.BLL.DTO.ActionEntity? Map(App.DTO.v1.ActionEntity? entity)
    {
        if (entity == null) return null;
        var res = new App.BLL.DTO.ActionEntity()
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
        return res;
    }
    
    public App.BLL.DTO.ActionEntity Map(App.DTO.v1.ActionEntityCreate entity)
    {
        var res = new App.BLL.DTO.ActionEntity()
        {
            Id = Guid.NewGuid(),
            Quantity = entity.Quantity,
            Status = entity.Status,
            ActionTypeId = entity.ActionTypeId,
            ReasonId = entity.ReasonId,
            SupplierId = entity.SupplierId,
            ProductId = entity.ProductId,
            StockAuditId = entity.StockAuditId,
            StorageRoomId = entity.StorageRoomId,
        };
        return res;
    }
}