using App.DTO.v1.ApiEntities;
using Base.Contracts;

namespace App.DTO.v1.ApiMapper;

/// <summary>
/// Maps between DTO V1 and BLL DTO ActionEntity objects.
/// Includes both full and simplified mapping logic.
/// </summary>
public class EnrichedActionEntityApiMapper : IMapper<EnrichedActionEntity, BLL.DTO.ActionEntity>
{
    /// <summary>
    /// Maps a full BLL DTO ActionEntity to a DTO V1 ActionEntity, including related objects.
    /// </summary>
    public EnrichedActionEntity? Map(BLL.DTO.ActionEntity? entity)
    {
        if (entity == null) return null;

        return new EnrichedActionEntity
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            Status = entity.Status,
            ActionTypeId = entity.ActionTypeId,
            ActionTypeName = entity.ActionType?.Name ?? "Unknown",
            ReasonId = entity.ReasonId,
            ReasonDescription = entity.Reason?.Description ?? "Unknown",
            ProductId = entity.ProductId,
            ProductName = entity.Product?.Name ?? "Unknown",
            ProductUnit = entity.Product?.Unit ?? "Unknown",
            StorageRoomId = entity.StorageRoomId,
            StorageRoomName = entity.StorageRoom?.Name ?? "Unknown",
            
            CreatedBy = entity.CreatedBy,
            CreatedAt = entity.CreatedAt,
        };
    }

    public BLL.DTO.ActionEntity? Map(EnrichedActionEntity? entity)
    {
        throw new NotImplementedException("Mapping from API to BLL is not required.");
    }
}
