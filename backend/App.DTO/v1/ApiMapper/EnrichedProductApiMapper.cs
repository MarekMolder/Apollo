using App.DTO.v1.ApiEntities;
using Base.Contracts;

namespace App.DTO.v1.ApiMapper;

public class EnrichedProductApiMapper : IMapper<EnrichedProduct, BLL.DTO.Product>
{
    public EnrichedProduct? Map(BLL.DTO.Product? entity)
    {
        if (entity == null) return null;

        return new EnrichedProduct
        {
            Id = entity.Id,
            Unit = entity.Unit,
            Volume = entity.Volume,
            Code = entity.Code,
            Name = entity.Name,
            Price = entity.Price,
            Quantity = entity.Quantity,
            ProductCategoryId = entity.ProductCategoryId,
            ProductCategoryName = entity.ProductCategory?.Name ?? "Unknown",
        };
    }

    public BLL.DTO.Product? Map(EnrichedProduct? entity)
    {
        throw new NotImplementedException("Mapping from API to BLL is not required.");
    }
    
}