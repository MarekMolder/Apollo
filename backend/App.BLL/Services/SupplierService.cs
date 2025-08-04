using App.BLL.Contracts;
using App.BLL.DTO;
using App.DAL.Contracts;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

/// <summary>
/// Business logic service for managing Suppliers.
/// Provides enriched supplier data including related Address and Products.
/// </summary>
public class SupplierService : BaseService<BLL.DTO.Supplier, DAL.DTO.Supplier, ISupplierRepository>, ISupplierService
{
    private readonly IMapper<BLL.DTO.Supplier, DAL.DTO.Supplier> _dalBllMapperSuppliers;
    public SupplierService(IAppUOW serviceUow, IMapper<BLL.DTO.Supplier, DAL.DTO.Supplier> bllMapperSuppliers) : base(serviceUow, serviceUow.SupplierRepository, bllMapperSuppliers)
    {
        _dalBllMapperSuppliers = bllMapperSuppliers;
    }
    
    /// <summary>
    /// Returns Suppliers enriched with related data (joins from DB).
    /// </summary>
    public async Task<IEnumerable<BLL.DTO.Supplier?>> GetEnrichedSuppliers()
    {
        var res = await ServiceRepository.GetEnrichedSuppliers();
        return res.Select(u => _dalBllMapperSuppliers.Map(u));
    }
}
