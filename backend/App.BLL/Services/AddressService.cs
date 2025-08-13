using App.BLL.Contracts;
using App.BLL.Mappers;
using App.DAL.Contracts;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

/// <summary>
/// Business logic service for managing Addresses.
/// </summary>
public class AddressService : BaseService<BLL.DTO.Address, DAL.DTO.Address, IAddressRepository>, IAddressService
{
    // Maps between DAL.DTO and BLL.DTO Address
    private readonly IMapper<BLL.DTO.Address, DAL.DTO.Address> _dalBllMapperAddress;
    
    public AddressService(IAppUOW serviceUow, IMapper<BLL.DTO.Address, DAL.DTO.Address> bllMapperAddress) : base(serviceUow, serviceUow.AddressRepository, bllMapperAddress)
    {
        _dalBllMapperAddress = bllMapperAddress;
    }
    
    /// <summary>
    /// Returns Addresses enriched with related data (joins from DB).
    /// </summary>
    public async Task<IEnumerable<BLL.DTO.Address?>> GetEnrichedAddresses()
    {
        var res = await ServiceRepository.GetEnrichedAddresses();
        return res.Select(u => _dalBllMapperAddress.Map(u));
    }
}
