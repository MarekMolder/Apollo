using Base.DAL.Contracts;

namespace App.DAL.Contracts;

/// <summary>
/// Repository interface for handling data access related to Suppliers.
/// </summary>
public interface ISupplierRepository: IBaseRepository<DAL.DTO.Supplier>
{
    /// <summary>
    /// Retrieves Supplier enriched with related data.
    /// </summary>
    Task<IEnumerable<DAL.DTO.Supplier?>> GetEnrichedSuppliers();
}