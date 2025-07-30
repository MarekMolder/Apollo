using Base.Contracts;

namespace App.DTO.v1.Mappers;

/// <summary>
/// Maps between DTO V1 and BLL DTO ActionEntity objects.
/// Includes both full and simplified mapping logic.
/// </summary>
public class ActionEntityApiMapper : IMapper<ActionEntity, BLL.DTO.ActionEntity>
{
    /// <summary>
    /// Maps a simplified BLL DTO ActionEntity to DTO V1 ActionEntity, without related objects.
    /// </summary>
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
            ProductId = entity.ProductId,
            StorageRoomId = entity.StorageRoomId,
        };
        return res;
    }

    /// <summary>
    /// Maps a simplified DTO V1 ActionEntity to BLL DTO ActionEntity, without related objects.
    /// </summary>
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
            ProductId = entity.ProductId,
            StorageRoomId = entity.StorageRoomId,
        };
        return res;
    }
    
    /// <summary>
    /// Maps a simplified DTO V1 ActionEntityCreate to BLL DTO ActionEntity, without related objects.
    /// </summary>
    public BLL.DTO.ActionEntity Map(ActionEntityCreate entity)
    {
        var res = new BLL.DTO.ActionEntity
        {
            Id = Guid.NewGuid(),
            Quantity = entity.Quantity,
            Status = entity.Status,
            ActionTypeId = entity.ActionTypeId,
            ReasonId = entity.ReasonId,
            ProductId = entity.ProductId,
            StorageRoomId = entity.StorageRoomId,
        };
        return res;
    }
}