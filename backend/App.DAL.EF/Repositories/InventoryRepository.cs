using App.DAL.Contracts;
using App.DAL.EF.Mappers;
using App.Domain.Logic;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class InventoryRepository: BaseRepository<App.DAL.DTO.Inventory, App.Domain.Logic.Inventory>, IInventoryRepository
{
    public InventoryRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new InventoryUOWMapper())
    {
    }
    public async Task<IEnumerable<App.DAL.DTO.Inventory?>> GetEnrichedInventories()
    {
        var domainEntities = await RepositoryDbSet
            .Include(a => a.Address)
            .ToListAsync();

        return domainEntities.Select(e => Mapper.Map(e));
    }
}