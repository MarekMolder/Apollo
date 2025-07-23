using App.DAL.Contracts;
using App.DAL.EF.Mappers;
using App.Resources.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using StockMovement = App.Domain.Logic.StockMovement;

namespace App.DAL.EF.Repositories;

public class StockMovementRepository: BaseRepository<App.DAL.DTO.StockMovement, App.Domain.Logic.StockMovement>, IStockMovementRepository
{
    public StockMovementRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new StockMovementUOWMapper())
    {
    }
}