using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class ActionEntityAPIMapper : IMapper<ActionEntity, BLL.DTO.ActionEntity>
{
    public ActionEntity? Map(BLL.DTO.ActionEntity? entity)
    {
        if (entity == null) return null;
        var res = new ActionEntity
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
        return res;
    }

    public BLL.DTO.ActionEntity? Map(ActionEntity? entity)
    {
        if (entity == null) return null;
        var res = new BLL.DTO.ActionEntity
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
        return res;
    }
    
    public BLL.DTO.ActionEntity Map(ActionEntityCreate entity)
    {
        var res = new BLL.DTO.ActionEntity
        {
            Id = Guid.NewGuid(),
            Quantity = entity.Quantity,
            Status = entity.Status,
            ActionTypeId = entity.ActionTypeId,
            ReasonId = entity.ReasonId,
            SupplierId = entity.SupplierId,
            ProductId = entity.ProductId,
            StorageRoomId = entity.StorageRoomId,
        };
        return res;
    }
}