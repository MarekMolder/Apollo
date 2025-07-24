using App.DTO.v1.ApiEntities;
using Base.Contracts;

namespace App.DTO.v1.ApiMappers;

public class EnrichedActionEntityApiMapper : IMapper<ApiEntities.EnrichedActionEntity, App.BLL.DTO.ActionEntity>
{
    public EnrichedActionEntity? Map(App.BLL.DTO.ActionEntity? entity)
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
            SupplierId = entity.SupplierId,
            SupplierName = entity.Supplier?.Name ?? "Unknown",
            ProductId = entity.ProductId,
            ProductName = entity.Product?.Name ?? "Unknown",
            StorageRoomId = entity.StorageRoomId,
            StorageRoomName = entity.StorageRoom?.Name ?? "Unknown",
        };
    }

    public App.BLL.DTO.ActionEntity? Map(EnrichedActionEntity? entity)
    {
        throw new NotImplementedException("Mapping from API to BLL is not required.");
    }
}
