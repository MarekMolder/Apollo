using App.DAL.Contracts;
using App.DAL.EF.Mappers;
using App.Domain.Logic;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ActionTypeEntityRepository : BaseRepository<App.DAL.DTO.ActionTypeEntity, App.Domain.Logic.ActionTypeEntity>, IActionTypeEntityRepository
{
    public ActionTypeEntityRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new ActionTypeEntityUOWMapper())
    {
    }
}