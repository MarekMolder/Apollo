using Base.BLL.Contracts;

namespace App.BLL.Contracts;

public interface IProductService : IBaseService<DTO.Product>
{
    Task<IEnumerable<DTO.Product?>> GetEnrichedProducts();
    
}