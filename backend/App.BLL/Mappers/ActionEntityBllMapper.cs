using Base.Contracts;

namespace App.BLL.Mappers;

/// <summary>
/// Maps between BLL DTO and DAL DTO ActionEntity objects.
/// Includes both full and simplified mapping logic.
/// </summary>
public class ActionEntityBllMapper : IMapper<App.BLL.DTO.ActionEntity, DAL.DTO.ActionEntity>
{
    /// <summary>
    /// Maps a full BLL DTO ActionEntity to a DAL DTO ActionEntity, including related objects.
    /// </summary>
    public DAL.DTO.ActionEntity? Map(BLL.DTO.ActionEntity? entity)
    {
        if (entity == null) return null;

        var res = new DAL.DTO.ActionEntity()
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            Status = entity.Status,
            
            ActionTypeId = entity.ActionTypeId,
            ActionType = ActionTypeEntityBllMapper.MapSimple(entity.ActionType),
            
            ReasonId = entity.ReasonId,
            Reason = ReasonBllMapper.MapSimple(entity.Reason),
            
            ProductId = entity.ProductId,
            Product = ProductBllMapper.MapSimple(entity.Product),
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomBllMapper.MapSimple(entity.StorageRoom),
            
            CreatedBy = entity.CreatedBy,
            CreatedAt = entity.CreatedAt,
            ChangedBy = entity.ChangedBy,
            ChangedAt = entity.ChangedAt,
        };
        return res;
    }

    /// <summary>
    /// Maps a full DAL DTO ActionEntity to a BLL DTO ActionEntity, including related objects.
    /// </summary
    public BLL.DTO.ActionEntity? Map(DAL.DTO.ActionEntity? entity)
    {
        if (entity == null) return null;

        var res = new BLL.DTO.ActionEntity()
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            Status = entity.Status,
            
            ActionTypeId = entity.ActionTypeId,
            ActionType = ActionTypeEntityBllMapper.MapSimple(entity.ActionType),
            
            ReasonId = entity.ReasonId,
            Reason = ReasonBllMapper.MapSimple(entity.Reason),
            
            ProductId = entity.ProductId,
            Product = ProductBllMapper.MapSimple(entity.Product),
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomBllMapper.MapSimple(entity.StorageRoom),
            
            CreatedBy = entity.CreatedBy,
            CreatedAt = entity.CreatedAt,
            ChangedBy = entity.ChangedBy,
            ChangedAt = entity.ChangedAt,
        };
        return res;
    }
    
    /// <summary>
    /// Maps a simplified BLL DTO ActionEntity to DAL DTO ActionEntity, without related objects.
    /// </summary>
    public static DAL.DTO.ActionEntity? MapSimple(BLL.DTO.ActionEntity? entity)
    {
        if (entity == null) return null;

        return new DAL.DTO.ActionEntity()
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            Status = entity.Status,
            ActionTypeId = entity.ActionTypeId,
            ReasonId = entity.ReasonId,
            ProductId = entity.ProductId,
            StorageRoomId = entity.StorageRoomId,
            
            CreatedBy = entity.CreatedBy,
            CreatedAt = entity.CreatedAt,
            ChangedBy = entity.ChangedBy,
            ChangedAt = entity.ChangedAt,
        };
    }
    
    /// <summary>
    /// Maps a simplified DAL DTO ActionEntity to BLL DTO, without related objects.
    /// </summary>
    public static BLL.DTO.ActionEntity? MapSimple(DAL.DTO.ActionEntity? entity)
    {
        if (entity == null) return null;

        return new BLL.DTO.ActionEntity()
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            Status = entity.Status,
            ActionTypeId = entity.ActionTypeId,
            ReasonId = entity.ReasonId,
            ProductId = entity.ProductId,
            StorageRoomId = entity.StorageRoomId,
            
            CreatedBy = entity.CreatedBy,
            CreatedAt = entity.CreatedAt,
            ChangedBy = entity.ChangedBy,
            ChangedAt = entity.ChangedAt,
        };
    }
}
