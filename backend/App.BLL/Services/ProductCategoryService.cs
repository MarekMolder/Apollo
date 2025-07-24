using App.BLL.Contracts;
using App.BLL.DTO;
using App.BLL.Mappers;
using App.DAL.Contracts;
using Base.BLL;

namespace App.BLL.Services;

public class ProductCategoryService : BaseService<ProductCategory, DAL.DTO.ProductCategory, IProductCategoryRepository>, IProductCategoryService
{
    public ProductCategoryService(IAppUOW serviceUow, ProductCategoryBLLMapper bllMapper) : base(serviceUow, serviceUow.ProductCategoryRepository, bllMapper)
    {
    }
}