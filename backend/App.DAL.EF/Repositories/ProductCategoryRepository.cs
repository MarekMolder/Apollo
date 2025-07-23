using App.DAL.Contracts;
using App.DAL.EF.Mappers;
using App.Domain.Logic;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ProductCategoryRepository: BaseRepository<App.DAL.DTO.ProductCategory, App.Domain.Logic.ProductCategory>, IProductCategoryRepository
{
    public ProductCategoryRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new ProductCategoryUOWMapper())
    {
    }
}