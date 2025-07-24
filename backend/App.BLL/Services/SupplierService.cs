using App.BLL.Contracts;
using App.BLL.DTO;
using App.DAL.Contracts;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class SupplierService : BaseService<Supplier, DAL.DTO.Supplier, ISupplierRepository>, ISupplierService
{
    private readonly IMapper<Supplier, DAL.DTO.Supplier> _dalToBLLMapper;
    public SupplierService(IAppUOW serviceUow, IMapper<Supplier, DAL.DTO.Supplier> mapper) : base(serviceUow, serviceUow.SupplierRepository, mapper)
    {
        _dalToBLLMapper = mapper;
    }
    
    public async Task<IEnumerable<Supplier?>> GetEnrichedSuppliers()
    {
        var res = await ServiceRepository.GetEnrichedSuppliers();
        return res.Select(u => _dalToBLLMapper.Map(u));
    }
}