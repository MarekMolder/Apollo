using Base.BLL.Contracts;

namespace App.BLL.Contracts;

/// <summary>
/// Business logic contract for managing Supplier operations.
/// </summary>
public interface ISupplierService : IBaseService<BLL.DTO.Supplier>
{
    /// <summary>
    /// Retrieves Supplier enriched with related data.
    /// </summary>
    Task<IEnumerable<BLL.DTO.Supplier?>> GetEnrichedSuppliers();
}