using App.DAL.Contracts;
using App.DAL.DTO;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class InventoryRepository: BaseRepository<Inventory, Domain.Logic.Inventory>, IInventoryRepository
{
    public InventoryRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new InventoryUOWMapper())
    {
    }
    public async Task<IEnumerable<Inventory?>> GetEnrichedInventories()
    {
        var domainEntities = await RepositoryDbSet
            .Include(a => a.Address)
            .ToListAsync();

        return domainEntities.Select(e => Mapper.Map(e));
    }
}