using App.DTO.v1.ApiEntities;
using Base.Contracts;

namespace App.DTO.v1.ApiMappers;

public class EnrichedCurrentStockApiMapper : IMapper<ApiEntities.EnrichedCurrentStock, App.BLL.DTO.CurrentStock>
{
    public EnrichedCurrentStock? Map(App.BLL.DTO.CurrentStock? entity)
    {
        if (entity == null) return null;

        return new EnrichedCurrentStock
        {
            Id = entity.Id,
            Quantity = entity.Quantity,
            UpdatedAt = entity.UpdatedAt,
            ProductId = entity.ProductId,
            ProductCode = entity.Product?.Code ?? "Unknown",
            ProductName = entity.Product?.Name ?? "Unknown",
            StorageRoomId = entity.StorageRoomId,
        };
    }

    public App.BLL.DTO.CurrentStock? Map(EnrichedCurrentStock? entity)
    {
        throw new NotImplementedException("Mapping from API to BLL is not required.");
    }
    
}