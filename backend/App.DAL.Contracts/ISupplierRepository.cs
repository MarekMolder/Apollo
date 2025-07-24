using App.DAL.DTO;
using Base.DAL.Contracts;

namespace App.DAL.Contracts;

public interface ISupplierRepository: IBaseRepository<Supplier>
{
    Task<IEnumerable<Supplier?>> GetEnrichedSuppliers();
}