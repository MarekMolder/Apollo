using App.DAL.Contracts;
using App.DAL.DTO;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ActionTypeEntityRepository : BaseRepository<ActionTypeEntity, Domain.Logic.ActionTypeEntity>, IActionTypeEntityRepository
{
    public ActionTypeEntityRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new ActionTypeEntityUOWMapper())
    {
    }
}