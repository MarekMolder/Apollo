using Base.BLL.Contracts;

namespace App.BLL.Contracts;

public interface IProductService : IBaseService<App.BLL.DTO.Product>
{
    Task<IEnumerable<App.BLL.DTO.Product?>> GetEnrichedProducts();
    
}