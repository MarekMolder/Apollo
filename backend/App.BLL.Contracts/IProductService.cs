using Base.BLL.Contracts;

namespace App.BLL.Contracts;

/// <summary>
/// Business logic contract for managing Product operations.
/// </summary>
public interface IProductService : IBaseService<BLL.DTO.Product>
{
    /// <summary>
    /// Retrieves Products enriched with related data.
    /// </summary>
    Task<IEnumerable<BLL.DTO.Product?>> GetEnrichedProducts();
}
