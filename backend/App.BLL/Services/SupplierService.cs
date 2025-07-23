using App.BLL.Contracts;
using App.BLL.Mappers;
using App.DAL.Contracts;
using App.DAL.DTO;
using Base.BLL;
using Base.BLL.Contracts;
using Base.Contracts;
using Base.DAL.Contracts;

namespace App.BLL.Services;

public class SupplierService : BaseService<App.BLL.DTO.Supplier, App.DAL.DTO.Supplier, App.DAL.Contracts.ISupplierRepository>, ISupplierService
{
    private readonly IMapper<DTO.Supplier, Supplier> _dalToBLLMapper;
    public SupplierService(IAppUOW serviceUow, IMapper<DTO.Supplier, Supplier> mapper) : base(serviceUow, serviceUow.SupplierRepository, mapper)
    {
        _dalToBLLMapper = mapper;
    }
    
    public async Task<IEnumerable<App.BLL.DTO.Supplier?>> GetEnrichedSuppliers()
    {
        var res = await ServiceRepository.GetEnrichedSuppliers();
        return res.Select(u => _dalToBLLMapper.Map(u));
    }
}