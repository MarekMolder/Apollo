using Base.BLL.Contracts;

namespace App.BLL.Contracts;

public interface ISupplierService : IBaseService<DTO.Supplier>
{
    Task<IEnumerable<DTO.Supplier?>> GetEnrichedSuppliers();
}