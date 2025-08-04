using App.DAL.Contracts;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

/// <summary>
/// Repository implementation for accessing and querying ProductCategory data from the database.
/// </summary>
public class ProductCategoryRepository: BaseRepository<DAL.DTO.ProductCategory, Domain.Logic.ProductCategory>, IProductCategoryRepository
{
    public ProductCategoryRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new ProductCategoryUowMapper())
    {
    }
}