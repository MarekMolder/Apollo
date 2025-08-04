using Base.DAL.Contracts;

namespace App.DAL.Contracts;

/// <summary>
/// Repository interface for handling data access related to Products.
/// </summary>
public interface IProductRepository: IBaseRepository<DAL.DTO.Product>
{
    /// <summary>
    /// Retrieves Products enriched with related data.
    /// </summary>
    Task<IEnumerable<DAL.DTO.Product?>> GetEnrichedProducts();
    
    /// <summary>
    /// Retrieves all products associated with a given supplier ID.
    /// </summary>
    Task<IEnumerable<DAL.DTO.Product?>> GetProductsBySupplierAsync(Guid supplierId);
}