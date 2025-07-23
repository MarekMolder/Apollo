using App.DAL.Contracts;
using App.DAL.EF.Mappers;
using App.Domain.Logic;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class StorageRoomInInventoryRepository: BaseRepository<App.DAL.DTO.StorageRoomInInventory, App.Domain.Logic.StorageRoomInInventory>, IStorageRoomInInventoryRepository
{
    public StorageRoomInInventoryRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new StorageRoomInInventoryUOWMapper())
    {
    }
}