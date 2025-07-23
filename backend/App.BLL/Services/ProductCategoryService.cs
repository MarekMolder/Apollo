using App.BLL.Contracts;
using App.BLL.Mappers;
using App.DAL.Contracts;
using App.DAL.DTO;
using Base.BLL;
using Base.BLL.Contracts;
using Base.DAL.Contracts;

namespace App.BLL.Services;

public class ProductCategoryService : BaseService<App.BLL.DTO.ProductCategory, App.DAL.DTO.ProductCategory, App.DAL.Contracts.IProductCategoryRepository>, IProductCategoryService
{
    public ProductCategoryService(IAppUOW serviceUow, ProductCategoryBLLMapper bllMapper) : base(serviceUow, serviceUow.ProductCategoryRepository, bllMapper)
    {
    }
}