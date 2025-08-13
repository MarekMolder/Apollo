using Base.BLL.Contracts;

namespace App.BLL.Contracts;

/// <summary>
/// Business logic contract for managing Address operations.
/// </summary>
public interface IAddressService : IBaseService<BLL.DTO.Address>
{
    /// <summary>
    /// Retrieves Addresses enriched with related data.
    /// </summary>
    Task<IEnumerable<BLL.DTO.Address?>> GetEnrichedAddresses();
}