using App.DAL.Contracts;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

/// <summary>
/// Repository implementation for accessing and querying storage room data from the database.
/// Supports retrieval of enriched storage room data including related address information.
/// </summary>
public class StorageRoomRepository: BaseRepository<DAL.DTO.StorageRoom, Domain.Logic.StorageRoom>, IStorageRoomRepository
{
    private readonly AppDbContext _ctx;
    public StorageRoomRepository(AppDbContext ctx) : base(ctx, new StorageRoomUowMapper())
    {
        _ctx = ctx;
    }
    
    /// <summary>
    /// Retrieves all storage rooms with their associated address data.
    /// </summary>
    public async Task<IEnumerable<DAL.DTO.StorageRoom?>> GetEnrichedStorageRooms()
    {
        var domainEntities = await RepositoryDbSet
            .Include(a => a.Address)
            .ToListAsync();

        return domainEntities.Select(e => Mapper.Map(e));
    }
}