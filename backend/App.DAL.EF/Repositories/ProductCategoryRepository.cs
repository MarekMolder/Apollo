using App.DAL.Contracts;
using App.DAL.DTO;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ProductCategoryRepository: BaseRepository<ProductCategory, Domain.Logic.ProductCategory>, IProductCategoryRepository
{
    public ProductCategoryRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new ProductCategoryUOWMapper())
    {
    }
}