using App.DTO.v1.ApiEntities;
using Base.Contracts;

namespace App.DTO.v1.ApiMapper;

/// <summary>
/// Maps between DTO V1 and BLL DTO Product objects.
/// Includes both full and simplified mapping logic.
/// </summary>
public class EnrichedProductApiMapper : IMapper<EnrichedProduct, BLL.DTO.Product>
{
    /// <summary>
    /// Maps a full BLL DTO Product to a DTO V1 Product, including related objects.
    /// </summary>
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
            SupplierId = entity.SupplierId,
            SupplierName = entity.Supplier?.Name ?? "Unknown",
            SupplierEmail = entity.Supplier?.Email ?? "Unknown",
        };
    }

    public BLL.DTO.Product? Map(EnrichedProduct? entity)
    {
        throw new NotImplementedException("Mapping from API to BLL is not required.");
    }
}