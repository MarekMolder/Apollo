using App.BLL.Contracts;
using App.DAL.Contracts;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

/// <summary>
/// Business logic service for managing Products.
/// Provides access to enriched product data for UI or reporting.
/// </summary>
public class ProductService : BaseService<BLL.DTO.Product, DAL.DTO.Product, IProductRepository>, IProductService
{
    // Maps between DAL.DTO and BLL.DTO ActionEntity
    private readonly IMapper<BLL.DTO.Product, DAL.DTO.Product> _dalBllMapperProducts;
    public ProductService(IAppUOW serviceUow, IMapper<BLL.DTO.Product, DAL.DTO.Product> bllMapperProduct) : base(serviceUow, serviceUow.ProductRepository, bllMapperProduct)
    {
        _dalBllMapperProducts = bllMapperProduct;
    }
    
    /// <summary>
    /// Returns Products enriched with related data (joins from DB).
    /// </summary>
    public async Task<IEnumerable<BLL.DTO.Product?>> GetEnrichedProducts()
    {
        var res = await ServiceRepository.GetEnrichedProducts();
        return res.Select(u => _dalBllMapperProducts.Map(u));
    }
}
