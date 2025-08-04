using App.BLL.Contracts;
using App.BLL.Mappers;
using App.DAL.Contracts;
using Base.BLL;

namespace App.BLL.Services;

/// <summary>
/// Business logic service for managing ProductCategories.
/// </summary>
public class ProductCategoryService : BaseService<BLL.DTO.ProductCategory, DAL.DTO.ProductCategory, IProductCategoryRepository>, IProductCategoryService
{
    public ProductCategoryService(IAppUOW serviceUow, ProductCategoryBllMapper bllMapperProductCategory) : base(serviceUow, serviceUow.ProductCategoryRepository, bllMapperProductCategory)
    {
    }
}