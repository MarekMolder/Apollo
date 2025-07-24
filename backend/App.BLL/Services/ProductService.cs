using App.BLL.Contracts;
using App.BLL.DTO;
using App.DAL.Contracts;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class ProductService : BaseService<Product, DAL.DTO.Product, IProductRepository>, IProductService
{
    private readonly IMapper<Product, DAL.DTO.Product> _dalToBLLMapper;
    public ProductService(IAppUOW serviceUow, IMapper<Product, DAL.DTO.Product> mapper) : base(serviceUow, serviceUow.ProductRepository, mapper)
    {
        _dalToBLLMapper = mapper;
    }
    
    public async Task<IEnumerable<Product?>> GetEnrichedProducts()
    {
        var res = await ServiceRepository.GetEnrichedProducts();
        return res.Select(u => _dalToBLLMapper.Map(u));
    }
    
}