using App.BLL.Contracts;
using App.BLL.DTO;
using App.BLL.Mappers;
using App.DAL.Contracts;
using Base.BLL;

namespace App.BLL.Services;

public class AddressService : BaseService<Address, DAL.DTO.Address, IAddressRepository>, IAddressService
{
    public AddressService(IAppUOW serviceUow, AddressBLLMapper bllMapper) : base(serviceUow, serviceUow.AddressRepository, bllMapper)
    {
    }
}