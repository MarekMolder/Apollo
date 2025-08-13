using Base.DAL.Contracts;

namespace App.DAL.Contracts;

/// <summary>
/// Repository interface for handling data access related to Addresses.
/// </summary>
public interface IAddressRepository: IBaseRepository<DAL.DTO.Address>
{
    /// <summary>
    /// Retrieves Addresses enriched with related data.
    /// </summary>
    Task<IEnumerable<DAL.DTO.Address?>> GetEnrichedAddresses();
}