using App.DAL.Contracts;
using App.DAL.EF.Mappers;
using App.Domain.Logic;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class StockAuditRepository: BaseRepository<App.DAL.DTO.StockAudit, App.Domain.Logic.StockAudit>, IStockAuditRepository
{
    public StockAuditRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new StockAuditUOWMapper())
    {
    }
}