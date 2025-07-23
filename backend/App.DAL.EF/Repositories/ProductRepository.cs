using App.DAL.Contracts;
using App.DAL.EF.Mappers;
using App.Domain.Logic;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ProductRepository: BaseRepository<App.DAL.DTO.Product, App.Domain.Logic.Product>, IProductRepository
{
    public ProductRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new ProductUOWMapper())
    {
    }
    
    public async Task<IEnumerable<App.DAL.DTO.Product?>> GetEnrichedProducts()
    {
        var domainEntities = await RepositoryDbSet
            .Include(a => a.ProductCategory)
            .ToListAsync();

        return domainEntities.Select(e => Mapper.Map(e));
    }
}