using App.BLL.Contracts;
using App.BLL.Mappers;
using App.DAL.Contracts;
using Base.BLL;

namespace App.BLL.Services;

/// <summary>
/// Business logic service for managing Addresses.
/// </summary>
public class AddressService : BaseService<BLL.DTO.Address, DAL.DTO.Address, IAddressRepository>, IAddressService
{
    public AddressService(IAppUOW serviceUow, AddressBllMapper bllMapperAddress) : base(serviceUow, serviceUow.AddressRepository, bllMapperAddress)
    {
    }
}
