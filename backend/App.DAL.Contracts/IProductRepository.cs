using App.Domain.Logic;
using Base.DAL.Contracts;

namespace App.DAL.Contracts;

public interface IProductRepository: IBaseRepository<App.DAL.DTO.Product>
{
    Task<IEnumerable<App.DAL.DTO.Product?>> GetEnrichedProducts();
}