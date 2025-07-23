using Base.BLL.Contracts;

namespace App.BLL.Contracts;

public interface ISupplierService : IBaseService<App.BLL.DTO.Supplier>
{
    Task<IEnumerable<App.BLL.DTO.Supplier?>> GetEnrichedSuppliers();
}