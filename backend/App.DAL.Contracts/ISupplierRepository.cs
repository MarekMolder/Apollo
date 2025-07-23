using App.Domain.Logic;
using Base.DAL.Contracts;

namespace App.DAL.Contracts;

public interface ISupplierRepository: IBaseRepository<App.DAL.DTO.Supplier>
{
    Task<IEnumerable<App.DAL.DTO.Supplier?>> GetEnrichedSuppliers();
}