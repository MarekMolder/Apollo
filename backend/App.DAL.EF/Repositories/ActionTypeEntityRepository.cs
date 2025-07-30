using App.DAL.Contracts;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

/// <summary>
/// Repository implementation for accessing and querying ActionTypeEntity data from the database.
/// </summary>
public class ActionTypeEntityRepository : BaseRepository<DAL.DTO.ActionTypeEntity, Domain.Logic.ActionTypeEntity>, IActionTypeEntityRepository
{
    public ActionTypeEntityRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new ActionTypeEntityUowMapper())
    {
    }
}