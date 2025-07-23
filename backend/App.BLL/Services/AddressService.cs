using App.BLL.Contracts;
using App.BLL.Mappers;
using App.DAL.Contracts;
using App.DAL.DTO;
using Base.BLL;
using Base.BLL.Contracts;
using Base.DAL.Contracts;

namespace App.BLL.Services;

public class AddressService : BaseService<App.BLL.DTO.Address, App.DAL.DTO.Address, App.DAL.Contracts.IAddressRepository>, IAddressService
{
    public AddressService(IAppUOW serviceUow, AddressBLLMapper bllMapper) : base(serviceUow, serviceUow.AddressRepository, bllMapper)
    {
    }
}