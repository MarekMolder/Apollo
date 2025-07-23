using App.DAL.Contracts;
using App.DAL.EF.Mappers;
using App.Domain.Logic;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ReasonRepository: BaseRepository<App.DAL.DTO.Reason, App.Domain.Logic.Reason>, IReasonRepository
{
    public ReasonRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new ReasonUOWMapper())
    {
    }
}