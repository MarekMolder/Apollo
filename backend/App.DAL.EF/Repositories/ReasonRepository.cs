using App.DAL.Contracts;
using App.DAL.DTO;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ReasonRepository: BaseRepository<Reason, Domain.Logic.Reason>, IReasonRepository
{
    public ReasonRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new ReasonUOWMapper())
    {
    }
}