using App.DAL.Contracts;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

/// <summary>
/// Repository implementation for accessing and querying Product data from the database.
/// Supports enriched data retrieval including related product category information.
/// </summary>
public class ProductRepository: BaseRepository<DAL.DTO.Product, Domain.Logic.Product>, IProductRepository
{
    public ProductRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new ProductUowMapper())
    {
    }
    
    /// <summary>
    /// Retrieves all products with their associated product category.
    /// </summary>
    public async Task<IEnumerable<DAL.DTO.Product?>> GetEnrichedProducts()
    {
        var domainEntities = await RepositoryDbSet
            .Include(a => a.ProductCategory)
            .ToListAsync();

        return domainEntities.Select(e => Mapper.Map(e));
    }
    
    /// <summary>
    /// Retrieves all products with their associated supplier.
    /// </summary>
    public async Task<IEnumerable<DAL.DTO.Product?>> GetProductsBySupplierAsync(Guid supplierId)
    {
        var domainEntities = await RepositoryDbSet
            .Where(p => p.SupplierId == supplierId)
            .Include(p => p.ProductCategory)
            .ToListAsync();

        return domainEntities.Select(p => Mapper.Map(p));
    }
}