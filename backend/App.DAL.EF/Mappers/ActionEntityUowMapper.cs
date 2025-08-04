using Base.Contracts;

namespace App.DAL.EF.Mappers;

/// <summary>
/// Maps between DAL DTO and Domain ActionEntity objects.
/// Includes both full and simplified mapping logic.
/// </summary>
public class ActionEntityUowMapper : IMapper<DAL.DTO.ActionEntity, Domain.Logic.ActionEntity>
{
    /// <summary>
    /// Maps a full Domain ActionEntity to a DAL DTO ActionEntity, including related objects.
    /// </summary>
    public DAL.DTO.ActionEntity? Map(Domain.Logic.ActionEntity? entity)
    {
        if (entity == null) return null;

        var res = new DAL.DTO.ActionEntity
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            Status = entity.Status,
            
            ActionTypeId = entity.ActionTypeId,
            ActionType = ActionTypeEntityUowMapper.MapSimple(entity.ActionType),
            
            ReasonId = entity.ReasonId,
            Reason = ReasonUowMapper.MapSimple(entity.Reason),
            
            ProductId = entity.ProductId,
            Product = ProductUowMapper.MapSimple(entity.Product),
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomUowMapper.MapSimple(entity.StorageRoom),
            
        };
        return res;
    }

    /// <summary>
    /// Maps a full DAL DTO ActionEntity to a Domain ActionEntity, including related objects.
    /// </summary
    public Domain.Logic.ActionEntity? Map(DAL.DTO.ActionEntity? entity)
    {
        if (entity == null) return null;

        var res = new Domain.Logic.ActionEntity
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            Status = entity.Status,
            
            ActionTypeId = entity.ActionTypeId,
            ActionType = ActionTypeEntityUowMapper.MapSimple(entity.ActionType),
            
            ReasonId = entity.ReasonId,
            Reason = ReasonUowMapper.MapSimple(entity.Reason),
            
            ProductId = entity.ProductId,
            Product = ProductUowMapper.MapSimple(entity.Product),
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomUowMapper.MapSimple(entity.StorageRoom),
        };
        return res;
    }
    
    /// <summary>
    /// Maps a simplified Domain ActionEntity to DAL DTO ActionEntity, without related objects.
    /// </summary>
    public static DAL.DTO.ActionEntity? MapSimple(Domain.Logic.ActionEntity? entity)
    {
        if (entity == null) return null;

        return new DAL.DTO.ActionEntity
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            Status = entity.Status,
            ActionTypeId = entity.ActionTypeId,
            ReasonId = entity.ReasonId,
            ProductId = entity.ProductId,
            StorageRoomId = entity.StorageRoomId,
        };
    }
    
    /// <summary>
    /// Maps a simplified DAL DTO ActionEntity to Domain, without related objects.
    /// </summary>
    public static Domain.Logic.ActionEntity? MapSimple(DAL.DTO.ActionEntity? entity)
    {
        if (entity == null) return null;

        return new Domain.Logic.ActionEntity
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            Status = entity.Status,
            ActionTypeId = entity.ActionTypeId,
            ReasonId = entity.ReasonId,
            ProductId = entity.ProductId,
            StorageRoomId = entity.StorageRoomId,
        };
    }
}
