using App.DAL.Contracts;
using App.DAL.EF.Mappers;
using App.Domain.Logic;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class SupplierRepository: BaseRepository<App.DAL.DTO.Supplier, App.Domain.Logic.Supplier>, ISupplierRepository
{
    public SupplierRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new SupplierUOWMapper())
    {
    }
    
    public async Task<IEnumerable<App.DAL.DTO.Supplier?>> GetEnrichedSuppliers()
    {
        var domainEntities = await RepositoryDbSet
            .Include(a => a.Address)
            .ToListAsync();

        return domainEntities.Select(e => Mapper.Map(e));
    }
}