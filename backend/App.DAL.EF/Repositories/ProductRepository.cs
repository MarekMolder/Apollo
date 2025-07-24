using App.DAL.Contracts;
using App.DAL.DTO;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ProductRepository: BaseRepository<Product, Domain.Logic.Product>, IProductRepository
{
    public ProductRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new ProductUOWMapper())
    {
    }
    
    public async Task<IEnumerable<Product?>> GetEnrichedProducts()
    {
        var domainEntities = await RepositoryDbSet
            .Include(a => a.ProductCategory)
            .ToListAsync();

        return domainEntities.Select(e => Mapper.Map(e));
    }
}