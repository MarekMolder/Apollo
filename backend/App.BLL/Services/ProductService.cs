using App.BLL.Contracts;
using App.BLL.Mappers;
using App.DAL.Contracts;
using App.DAL.DTO;
using Base.BLL;
using Base.BLL.Contracts;
using Base.Contracts;
using Base.DAL.Contracts;

namespace App.BLL.Services;

public class ProductService : BaseService<App.BLL.DTO.Product, App.DAL.DTO.Product, App.DAL.Contracts.IProductRepository>, IProductService
{
    private readonly IMapper<DTO.Product, Product> _dalToBLLMapper;
    public ProductService(IAppUOW serviceUow, IMapper<DTO.Product, Product> mapper) : base(serviceUow, serviceUow.ProductRepository, mapper)
    {
        _dalToBLLMapper = mapper;
    }
    
    public async Task<IEnumerable<App.BLL.DTO.Product?>> GetEnrichedProducts()
    {
        var res = await ServiceRepository.GetEnrichedProducts();
        return res.Select(u => _dalToBLLMapper.Map(u));
    }
    
}