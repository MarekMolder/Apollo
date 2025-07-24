using App.DAL.Contracts;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using StorageRoom = App.Domain.Logic.StorageRoom;

namespace App.DAL.EF.Repositories;

public class StorageRoomRepository: BaseRepository<DTO.StorageRoom, StorageRoom>, IStorageRoomRepository
{
    private readonly AppDbContext _ctx;
    public StorageRoomRepository(AppDbContext ctx) : base(ctx, new StorageRoomUOWMapper())
    {
        _ctx = ctx;
    }
    
    public async Task<IEnumerable<DTO.StorageRoom>> GetAllByInventoryIdAsync(Guid inventoryId)
    {
        var domainEntities = await _ctx.StorageRoomInInventories
            .Where(x => x.InventoryId == inventoryId)
            .Include(x => x.StorageRoom)
            .Select(x => x.StorageRoom)
            .ToListAsync();

        return domainEntities.Select(e => Mapper.Map(e)!);
    }
    
}