using App.DAL.Contracts;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

/// <summary>
/// Repository implementation for accessing and querying Reason data from the database.
/// </summary>
public class ReasonRepository: BaseRepository<DAL.DTO.Reason, Domain.Logic.Reason>, IReasonRepository
{
    public ReasonRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new ReasonUowMapper())
    {
    }
}