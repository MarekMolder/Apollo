using App.DAL.Contracts;
using App.DAL.DTO;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class StorageRoomInInventoryRepository: BaseRepository<StorageRoomInInventory, Domain.Logic.StorageRoomInInventory>, IStorageRoomInInventoryRepository
{
    public StorageRoomInInventoryRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new StorageRoomInInventoryUOWMapper())
    {
    }
}